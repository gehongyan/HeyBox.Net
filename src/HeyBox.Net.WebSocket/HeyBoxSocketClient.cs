using System.Collections.Concurrent;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.API;
using HeyBox.API.Gateway;
using HeyBox.Logging;
using HeyBox.Net;
using HeyBox.Net.Queue;
using HeyBox.Net.WebSockets;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的 HeyBox 客户端。
/// </summary>
public partial class HeyBoxSocketClient : BaseSocketClient, IHeyBoxClient
{
    private readonly JsonSerializerOptions _serializerOptions;

    private readonly ConcurrentQueue<long> _heartbeatTimes;
    private readonly Logger _gatewayLogger;
    private readonly SemaphoreSlim _stateLock;

    private ulong _lastSeq;
    internal Task? _heartbeatTask;
    private long _lastMessageTime;

    private bool _isDisposed;

    /// <inheritdoc />
    public override HeyBoxSocketRestClient Rest { get; }

    internal virtual ConnectionManager Connection { get; }

    /// <inheritdoc />
    public override ConnectionState ConnectionState => Connection.State;

    /// <inheritdoc />
    public override int Latency { get; protected set; }

    internal int MessageCacheSize { get; private set; }
    internal ClientState State { get; private set; }
    internal WebSocketProvider WebSocketProvider { get; private set; }
    internal BaseMessageQueue MessageQueue { get; private set; }
    internal int? HandlerTimeout { get; private set; }
    internal new HeyBoxSocketApiClient ApiClient => base.ApiClient;


    /// <summary>
    ///     初始化一个 <see cref="HeyBoxSocketClient" /> 类的新实例。
    /// </summary>
    public HeyBoxSocketClient() : this(new HeyBoxSocketConfig())
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="HeyBoxSocketClient" /> 类的新实例。
    /// </summary>
    /// <param name="config"> 用于配置此客户端的配置对象。 </param>
    public HeyBoxSocketClient(HeyBoxSocketConfig config) : this(config, CreateApiClient(config))
    {
    }

    internal HeyBoxSocketClient(HeyBoxSocketConfig config, HeyBoxSocketApiClient client)
        : base(config, client)
    {

        WebSocketProvider = config.WebSocketProvider;
        MessageQueue = config.MessageQueueProvider(ProcessGatewayEventAsync);
        HandlerTimeout = config.HandlerTimeout;
        State = new ClientState(0);
        Rest = new HeyBoxSocketRestClient(config, ApiClient);
        _heartbeatTimes = new ConcurrentQueue<long>();

        _stateLock = new SemaphoreSlim(1, 1);
        _gatewayLogger = LogManager.CreateLogger("Gateway");
        ConnectionManager connectionManager = new(_stateLock, _gatewayLogger, config.ConnectionTimeout,
            OnConnectingAsync, OnDisconnectingAsync, x => ApiClient.Disconnected += x);
        connectionManager.Connected += () => TimedInvokeAsync(_connectedEvent, nameof(Connected));
        connectionManager.Disconnected += (ex, _) => TimedInvokeAsync(_disconnectedEvent, nameof(Disconnected), ex);
        Connection = connectionManager;

        _serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        ApiClient.SentGatewayMessage += async socketFrameType =>
            await _gatewayLogger.DebugAsync($"Sent {socketFrameType}").ConfigureAwait(false);
        ApiClient.ReceivedGatewayEvent += ProcessMessageAsync;
    }

    private static HeyBoxSocketApiClient CreateApiClient(HeyBoxSocketConfig config) =>
        new(config.RestClientProvider, config.WebSocketProvider, HeyBoxConfig.UserAgent,
            config.GatewayHost, defaultRatelimitCallback: config.DefaultRatelimitCallback);

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                try
                {
                    StopAsync().GetAwaiter().GetResult();
                }
                catch (NotSupportedException)
                {
                    // ignored
                }
                ApiClient?.Dispose();
                _stateLock?.Dispose();
            }

            _isDisposed = true;
        }

        base.Dispose(disposing);
    }

    private async Task OnConnectingAsync()
    {
        try
        {
            await _gatewayLogger.DebugAsync("Connecting ApiClient").ConfigureAwait(false);
            await ApiClient.ConnectAsync().ConfigureAwait(false);
            if (TokenUtils.TryParseBotTokenUserId(Token, out uint selfUserId))
            {
                SocketSelfUser currentUser = SocketSelfUser.Create(this, State, selfUserId);
                Rest.CreateRestSelfUser(currentUser.Id);
                ApiClient.CurrentUserId = currentUser.Id;
                Rest.CurrentUser = new RestSelfUser(this, currentUser.Id);
                CurrentUser = currentUser;
            }
            _heartbeatTask = RunHeartbeatAsync(Connection.CancellationToken);
            _ = Connection.CompleteAsync();
        }
        catch (HttpException ex)
        {
            if (ex.HttpCode == HttpStatusCode.Unauthorized)
                Connection.CriticalError(ex);
            else
                Connection.Error(ex);
        }
        catch
        {
            // ignored
        }

        await Connection.WaitAsync().ConfigureAwait(false);
    }

    private async Task OnDisconnectingAsync(Exception ex)
    {
        await _gatewayLogger.DebugAsync("Disconnecting ApiClient").ConfigureAwait(false);
        await ApiClient.DisconnectAsync(ex).ConfigureAwait(false);

        //Wait for tasks to complete
        await _gatewayLogger.DebugAsync("Waiting for heartbeater").ConfigureAwait(false);
        Task? heartbeatTask = _heartbeatTask;
        if (heartbeatTask != null)
            await heartbeatTask.ConfigureAwait(false);
        _heartbeatTask = null;
        while (_heartbeatTimes.TryDequeue(out _))
        {
            // flush the queue
        }

        ResetCounter();
    }

    private protected void ResetCounter()
    {
        _lastMessageTime = 0;
    }

    /// <inheritdoc />
    public override SocketRoom GetRoom(ulong id) => State.GetOrAddRoom(id, x => new SocketRoom(this, x));

    internal SocketRoom GetOrCreateRoom(ClientState state, RoomBaseInfo model) =>
        state.GetOrAddRoom(model.RoomId, _ => SocketRoom.Create(this, state, model));

    internal SocketGlobalUser GetOrCreateSelfUser(ClientState state, uint id) =>
        state.GetOrAddUser(id, _ =>
        {
            SocketGlobalUser user = new(this, id);
            user.GlobalUser.AddRef();
            return user;
        });

    internal SocketGlobalUser GetOrCreateUser(ClientState state, uint id) =>
        state.GetOrAddUser(id, x => new SocketGlobalUser(this, x));

    internal SocketGlobalUser GetOrCreateUser(ClientState state, SenderInfo model) =>
        state.GetOrAddUser(model.UserId, _ => SocketGlobalUser.Create(this, state, model));

    internal void RemoveUser(uint id) => State.RemoveUser(id);

    #region ProcessMessageAsync

    internal virtual async Task ProcessMessageAsync(IGatewaySocketFrame gatewaySocketFrame)
    {
        _lastMessageTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        try
        {
            switch (gatewaySocketFrame)
            {
                case GatewayPlainTextSocketFrame plainTextFrame:
                    if (plainTextFrame.Data is "pong")
                        await HandlePongAsync().ConfigureAwait(false);
                    break;
                case GatewayJsonSocketFrame jsonFrame:
                    _lastMessageTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    if (jsonFrame.Sequence < _lastSeq)
                    {
                        await _gatewayLogger
                            .WarningAsync($"Received an event with a sequence less than the last event. "
                                + $"Last Seq: {_lastSeq} Current Seq: {jsonFrame.Sequence}")
                            .ConfigureAwait(false);
                    }
                    else if (jsonFrame.Sequence == _lastSeq)
                    {
                        await _gatewayLogger
                            .WarningAsync($"Received an event with the same sequence {_lastSeq} as the last event, ignored.")
                            .ConfigureAwait(false);
                        break;
                    }
                    _lastSeq = jsonFrame.Sequence;
                    await MessageQueue
                        .EnqueueAsync(jsonFrame.Type, jsonFrame.Data, jsonFrame.Sequence, jsonFrame.Timestamp)
                        .ConfigureAwait(false);
                    break;
                default:
                    throw new NotSupportedException("Unknown frame type");
            }
        }
        catch (Exception ex)
        {
            await _gatewayLogger
                .ErrorAsync($"Error handling {gatewaySocketFrame}. Payload: {SerializePayload(gatewaySocketFrame)}", ex)
                .ConfigureAwait(false);
        }
    }

    internal async Task ProcessGatewayEventAsync(string type, JsonElement payload)
    {
        if (!Enum.TryParse(type, out GatewayEventType eventType))
        {
            await _gatewayLogger
                .WarningAsync($"Unknown Event Type. Payload: {SerializePayload(payload)}")
                .ConfigureAwait(false);
            return;
        }

        switch (eventType)
        {
            case GatewayEventType.SlashCommand:
                await HandleSlashCommand(payload).ConfigureAwait(false);
                break;
            default:
            {
                await _gatewayLogger
                    .WarningAsync($"Unknown Event Type ({eventType}). Payload: {SerializePayload(payload)}")
                    .ConfigureAwait(false);
            }
                break;
        }
    }

    #endregion

    /// <inheritdoc />
    public override async Task StartAsync()
    {
        await MessageQueue.StartAsync();
        await Connection.StartAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task StopAsync()
    {
        await Connection.StopAsync().ConfigureAwait(false);
        await MessageQueue.StopAsync();
    }

    private async Task RunHeartbeatAsync(CancellationToken cancellationToken)
    {
        int intervalMillis = BaseConfig.HeartbeatIntervalMilliseconds;
        try
        {
            await _gatewayLogger.DebugAsync("Heartbeat Started").ConfigureAwait(false);
            while (!cancellationToken.IsCancellationRequested)
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Did server respond to our last heartbeat
                if (_heartbeatTimes.IsEmpty && now - _lastMessageTime > intervalMillis + 1000.0 / 64
                    && ConnectionState == ConnectionState.Connected)
                {
                    Connection.Error(new GatewayReconnectException("Server missed last heartbeat"));
                    return;
                }

                _heartbeatTimes.Enqueue(now);
                try
                {
                    await ApiClient.SendHeartbeatAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await _gatewayLogger.WarningAsync("Heartbeat Errored", ex).ConfigureAwait(false);
                }

                await Task.Delay(intervalMillis, cancellationToken).ConfigureAwait(false);
            }

            await _gatewayLogger.DebugAsync("Heartbeat Stopped").ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await _gatewayLogger.DebugAsync("Heartbeat Stopped").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _gatewayLogger.ErrorAsync("Heartbeat Errored", ex).ConfigureAwait(false);
        }
    }
    internal async Task TimedInvokeAsync(AsyncEvent<Func<Task>> eventHandler, string name)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, eventHandler.InvokeAsync).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync().ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T>(AsyncEvent<Func<T, Task>> eventHandler, string name, T arg)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2>(AsyncEvent<Func<T1, T2, Task>> eventHandler, string name, T1 arg1,
        T2 arg2)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3>(AsyncEvent<Func<T1, T2, T3, Task>> eventHandler, string name,
        T1 arg1, T2 arg2, T3 arg3)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3, T4>(AsyncEvent<Func<T1, T2, T3, T4, Task>> eventHandler,
        string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3, arg4)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3, arg4).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3, T4, T5>(AsyncEvent<Func<T1, T2, T3, T4, T5, Task>> eventHandler,
        string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3, arg4, arg5)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false);
    }

    private async Task TimeoutWrap(string name, Func<Task> action)
    {
        try
        {
            if (!HandlerTimeout.HasValue)
            {
                await action().ConfigureAwait(false);
                return;
            }

            Task timeoutTask = Task.Delay(HandlerTimeout.Value);
            Task handlersTask = action();
            if (await Task.WhenAny(timeoutTask, handlersTask).ConfigureAwait(false) == timeoutTask)
            {
                await _gatewayLogger.WarningAsync($"A {name} handler is blocking the gateway task.")
                    .ConfigureAwait(false);
            }

            await handlersTask.ConfigureAwait(false); //Ensure the handler completes
        }
        catch (Exception ex)
        {
            await _gatewayLogger.WarningAsync($"A {name} handler has thrown an unhandled exception.", ex)
                .ConfigureAwait(false);
        }
    }

    #region Helpers

    private T? DeserializePayload<T>(JsonElement jsonElement)
    {
        if (jsonElement.Deserialize<T>(_serializerOptions) is { } x) return x;
        string payloadJson = SerializePayload(jsonElement);
        _gatewayLogger.ErrorAsync($"Failed to deserialize JSON element to type {typeof(T).Name}: {payloadJson}");
        return default;
    }

    private string SerializePayload(object payload)
    {
        return payload switch
        {
            GatewayPlainTextSocketFrame plainTextSocketFrame => plainTextSocketFrame.Data,
            GatewayJsonSocketFrame jsonSocketFrame => JsonSerializer.Serialize(jsonSocketFrame.Data, _serializerOptions),
            _ => JsonSerializer.Serialize(payload, _serializerOptions)
        };
    }

    #endregion
}
