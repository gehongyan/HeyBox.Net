#if DEBUG_PACKETS
using System.Diagnostics;
using System.Text.Encodings.Web;
#endif

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using HeyBox.API.Gateway;
using HeyBox.Net.Queue;
using HeyBox.Net.Rest;
using HeyBox.Net.WebSockets;
using HeyBox.WebSocket;

namespace HeyBox.API;

internal class HeyBoxSocketApiClient : HeyBoxRestApiClient
{
    protected static readonly JsonElement EmptyJsonElement = JsonDocument.Parse("{}").RootElement;

    public event Func<string, Task> SentGatewayMessage
    {
        add => _sentGatewayMessageEvent.Add(value);
        remove => _sentGatewayMessageEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<string, Task>> _sentGatewayMessageEvent = new();

    public event Func<IGatewaySocketFrame, Task> ReceivedGatewayEvent
    {
        add => _receivedGatewayEvent.Add(value);
        remove => _receivedGatewayEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<IGatewaySocketFrame, Task>> _receivedGatewayEvent = new();

    public event Func<Exception, Task> Disconnected
    {
        add => _disconnectedEvent.Add(value);
        remove => _disconnectedEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<Exception, Task>> _disconnectedEvent = new();

    private readonly bool _isExplicitUrl;
    private CancellationTokenSource? _connectCancellationToken;
    private string? _gatewayUrl;

#if DEBUG_PACKETS
    private readonly JsonSerializerOptions _debugJsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
#endif

    public ConnectionState ConnectionState { get; private set; }
    internal IWebSocketClient WebSocketClient { get; }

    public HeyBoxSocketApiClient(RestClientProvider restClientProvider,
        WebSocketProvider webSocketProvider,
        string userAgent, string? url = null,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        JsonSerializerOptions? serializerOptions = null,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
        : base(restClientProvider, userAgent,
            defaultRetryMode, serializerOptions, defaultRatelimitCallback)
    {
        _gatewayUrl = url;
        if (url != null)
            _isExplicitUrl = true;
        WebSocketClient = webSocketProvider();
        WebSocketClient.SetKeepAliveInterval(TimeSpan.Zero);
        WebSocketClient.TextMessage += OnTextMessage;
        WebSocketClient.BinaryMessage += OnBinaryMessage;
        WebSocketClient.Closed += async ex =>
        {
#if DEBUG_PACKETS
            Debug.WriteLine(ex);
#endif
            await DisconnectAsync().ConfigureAwait(false);
            await _disconnectedEvent.InvokeAsync(ex).ConfigureAwait(false);
        };
    }

    private static bool TryParseAsJsonElement(string message, [NotNullWhen(true)] out JsonElement? element)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        return TryParseAsJsonElement(bytes, 0, bytes.Length, out element);
    }

    private static bool TryParseAsJsonElement(byte[] data, int index, int count, [NotNullWhen(true)] out JsonElement? element)
    {
        try
        {
            Utf8JsonReader reader = new(data.AsSpan()[index..(index + count)]);
            return JsonElement.TryParseValue(ref reader, out element);
        }
        catch
        {
            element = null;
            return false;
        }
    }

    private async Task OnBinaryMessage(byte[] data, int index, int count)
    {
        if (!TryParseAsJsonElement(data, index, count, out JsonElement? jsonElement))
        {
#if DEBUG_PACKETS
            Debug.WriteLine($"[{DateTimeOffset.Now:HH:mm:ss}] <- [PLAIN] {Encoding.Default.GetString(data, index, count)}");
#endif
            await _receivedGatewayEvent
                .InvokeAsync(new GatewayPlainTextSocketFrame(Encoding.Default.GetString(data, index, count)))
                .ConfigureAwait(false);
            return;
        }

        GatewaySocketFrame? gatewaySocketFrame = jsonElement.Value.Deserialize<GatewaySocketFrame>(_serializerOptions);
        if (gatewaySocketFrame is not null)
        {
#if DEBUG_PACKETS
            string raw = Encoding.Default.GetString(data.AsSpan()[index..(index + count)]).TrimEnd('\n');
            string parsed = JsonSerializer
                .Serialize(gatewaySocketFrame.Data, _debugJsonSerializerOptions)
                .TrimEnd('\n');
            Debug.WriteLine($"""
                [{DateTimeOffset.Now:HH:mm:ss}] <- [JSON] [{gatewaySocketFrame.Type}] : #{gatewaySocketFrame.Sequence}
                [Raw] {raw}
                [Parsed] {parsed}
                """);
#endif
            GatewayJsonSocketFrame frame = new(gatewaySocketFrame.Type, gatewaySocketFrame.Sequence, gatewaySocketFrame.Data, gatewaySocketFrame.Timestamp);
            await _receivedGatewayEvent.InvokeAsync(frame).ConfigureAwait(false);
        }
    }

    private async Task OnTextMessage(string message)
    {
        if (!TryParseAsJsonElement(message, out JsonElement? jsonElement))
        {
#if DEBUG_PACKETS
            Debug.WriteLine($"[{DateTimeOffset.Now:HH:mm:ss}] <- [PLAIN] {message}");
#endif
            await _receivedGatewayEvent
                .InvokeAsync(new GatewayPlainTextSocketFrame(message))
                .ConfigureAwait(false);
            return;
        }

        GatewaySocketFrame? gatewaySocketFrame = jsonElement.Value.Deserialize<GatewaySocketFrame>(_serializerOptions);
        if (gatewaySocketFrame is null)
            return;
#if DEBUG_PACKETS
        string parsed = JsonSerializer
            .Serialize(gatewaySocketFrame.Data, _debugJsonSerializerOptions)
            .TrimEnd('\n');
        Debug.WriteLine($"""
            [{DateTimeOffset.Now:HH:mm:ss}] <- [JSON] [{gatewaySocketFrame.Type}] : #{gatewaySocketFrame.Sequence}
            [Raw] {message}
            [Parsed] {parsed}
            """);
#endif
        GatewayJsonSocketFrame frame = new(gatewaySocketFrame.Type, gatewaySocketFrame.Sequence, gatewaySocketFrame.Data, gatewaySocketFrame.Timestamp);
        await _receivedGatewayEvent.InvokeAsync(frame).ConfigureAwait(false);
    }

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _connectCancellationToken?.Dispose();
                WebSocketClient?.Dispose();
            }

            _isDisposed = true;
        }

        base.Dispose(disposing);
    }

    public async Task ConnectAsync()
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await ConnectInternalAsync().ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal override async Task ConnectInternalAsync()
    {
        if (LoginState != LoginState.LoggedIn)
            throw new InvalidOperationException("The client must be logged in before connecting.");
        if (WebSocketClient == null)
            throw new NotSupportedException("This client is not configured with WebSocket support.");
        RequestQueue.ClearGatewayBuckets();
        ConnectionState = ConnectionState.Connecting;
        try
        {
            _connectCancellationToken?.Dispose();
            _connectCancellationToken = new CancellationTokenSource();
            WebSocketClient.SetCancellationToken(_connectCancellationToken.Token);
            if (AuthToken == null)
                throw new InvalidOperationException("The client must be logged in before connecting.");
            WebSocketClient.SetHeader("token", GetPrefixedToken(AuthTokenType, AuthToken));

            if (!_isExplicitUrl || _gatewayUrl == null)
                _gatewayUrl = $"{HeyBoxSocketConfig.DefaultGatewayUrl}?{HeyBoxConfig.CommonQueryString}";
#if DEBUG_PACKETS
            Debug.WriteLine("Connecting to gateway: " + _gatewayUrl);
#endif
            await WebSocketClient.ConnectAsync(_gatewayUrl).ConfigureAwait(false);
            ConnectionState = ConnectionState.Connected;
        }
        catch (Exception)
        {
            if (!_isExplicitUrl)
                _gatewayUrl = null;

            await DisconnectInternalAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task DisconnectAsync(Exception? ex = null)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await DisconnectInternalAsync(ex).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal override async Task DisconnectInternalAsync(Exception? ex = null)
    {
        if (WebSocketClient == null)
            throw new NotSupportedException("This client is not configured with WebSocket support.");
        if (ConnectionState == ConnectionState.Disconnected)
            return;
        ConnectionState = ConnectionState.Disconnecting;

        if (ex is GatewayReconnectException)
            await WebSocketClient.DisconnectAsync(4000).ConfigureAwait(false);
        else
            await WebSocketClient.DisconnectAsync().ConfigureAwait(false);

        try
        {
            _connectCancellationToken?.Cancel(false);
        }
        catch
        {
            // ignored
        }

        ConnectionState = ConnectionState.Disconnected;
    }

    public async Task SendHeartbeatAsync(RequestOptions? options = null)
    {
        RequestOptions requestOptions = RequestOptions.CreateOrClone(options);
        await SendGatewayAsync("ping", requestOptions).ConfigureAwait(false);
    }

    public Task SendGatewayAsync(string plainText, RequestOptions options) =>
        SendGatewayInternalAsync(plainText, options);

    public Task SendGatewayAsync<T>(T payload, RequestOptions options) where T : notnull =>
        SendGatewayInternalAsync(SerializeJson(payload), options);

    private async Task SendGatewayInternalAsync(string payload, RequestOptions options)
    {
        CheckState();
        byte[] bytes = Encoding.UTF8.GetBytes(payload);

        options.IsGatewayBucket = true;
        options.BucketId ??= GatewayBucket.Get(GatewayBucketType.Unbucketed).Id;
        bool ignoreLimit = payload is "ping";
        await RequestQueue
            .SendAsync(new WebSocketRequest(WebSocketClient, bytes, true, ignoreLimit, options))
            .ConfigureAwait(false);
        await _sentGatewayMessageEvent.InvokeAsync(payload).ConfigureAwait(false);

#if DEBUG_PACKETS
        Debug.WriteLine($"[{DateTimeOffset.Now:HH:mm:ss}] -> {payload}".TrimEnd('\n'));
#endif
    }
}
