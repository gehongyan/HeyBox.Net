using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.Net;
using HeyBox.Net.Queue;
using HeyBox.Net.Rest;
using HeyBox.API.Rest;

namespace HeyBox.API;

internal class HeyBoxRestApiClient : IDisposable
{
    #region HeyBoxRestApiClient

    private static readonly ConcurrentDictionary<string, Func<BucketIds, BucketId>> _bucketIdGenerators = new();

    public event Func<HttpMethod, string, double, Task> SentRequest
    {
        add => _sentRequestEvent.Add(value);
        remove => _sentRequestEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<HttpMethod, string, double, Task>> _sentRequestEvent = new();

    protected readonly JsonSerializerOptions _serializerOptions;
    protected readonly SemaphoreSlim _stateLock;
    private readonly RestClientProvider _restClientProvider;

    protected bool _isDisposed;
    private CancellationTokenSource? _loginCancellationToken;

    public RetryMode DefaultRetryMode { get; }
    public string UserAgent { get; }

    internal RequestQueue RequestQueue { get; }
    public LoginState LoginState { get; private set; }
    public TokenType AuthTokenType { get; private set; }
    internal string? AuthToken { get; private set; }
    internal IRestClient RestClient { get; private set; }
    internal ulong? CurrentUserId { get; set; }
    internal Func<IRateLimitInfo, Task>? DefaultRatelimitCallback { get; set; }

    public HeyBoxRestApiClient(RestClientProvider restClientProvider,
        string userAgent,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        JsonSerializerOptions? serializerOptions = null,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
    {
        _restClientProvider = restClientProvider;
        UserAgent = userAgent;
        DefaultRetryMode = defaultRetryMode;
        _serializerOptions = serializerOptions
            ?? new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
        DefaultRatelimitCallback = defaultRatelimitCallback;

        RequestQueue = new RequestQueue();
        _stateLock = new SemaphoreSlim(1, 1);
        SetBaseUrl();
    }

    [MemberNotNull(nameof(RestClient))]
    internal void SetBaseUrl()
    {
        RestClient?.Dispose();
        RestClient = _restClientProvider(HeyBoxConfig.APIUrl);
        RestClient.SetHeader("Accept", "*/*");
        RestClient.SetHeader("User-Agent", UserAgent);
    }

    internal static string GetPrefixedToken(TokenType tokenType, string token) =>
        tokenType switch
        {
            TokenType.BotToken => token,
            _ => throw new ArgumentException("Unknown token type.", nameof(tokenType))
        };

    internal virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
            _isDisposed = true;
    }

    public void Dispose() => Dispose(true);

    public async Task LoginAsync(TokenType tokenType, string token)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LoginInternalAsync(tokenType, token).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    private async Task LoginInternalAsync(TokenType tokenType, string token)
    {
        if (LoginState != LoginState.LoggedOut)
            await LogoutInternalAsync().ConfigureAwait(false);

        LoginState = LoginState.LoggingIn;

        try
        {
            _loginCancellationToken?.Dispose();
            _loginCancellationToken = new CancellationTokenSource();

            AuthToken = null;
            await RequestQueue.SetCancellationTokenAsync(_loginCancellationToken.Token).ConfigureAwait(false);
            RestClient.SetCancellationToken(_loginCancellationToken.Token);

            AuthTokenType = tokenType;
            AuthToken = token.TrimEnd();
            RestClient.SetHeader("token", GetPrefixedToken(AuthTokenType, AuthToken));

            LoginState = LoginState.LoggedIn;
        }
        catch
        {
            await LogoutInternalAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LogoutInternalAsync().ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    private async Task LogoutInternalAsync()
    {
        //An exception here will lock the client into the unusable LoggingOut state, but that's probably fine since our client is in an undefined state too.
        if (LoginState == LoginState.LoggedOut) return;

        LoginState = LoginState.LoggingOut;

        try
        {
            _loginCancellationToken?.Cancel(false);
        }
        catch
        {
            // ignored
        }

        await DisconnectInternalAsync().ConfigureAwait(false);
        await RequestQueue.ClearAsync().ConfigureAwait(false);

        await RequestQueue.SetCancellationTokenAsync(CancellationToken.None).ConfigureAwait(false);
        RestClient.SetCancellationToken(CancellationToken.None);

        CurrentUserId = null;
        LoginState = LoginState.LoggedOut;
    }

    internal virtual Task ConnectInternalAsync() => Task.CompletedTask;
    internal virtual Task DisconnectInternalAsync(Exception? ex = null) => Task.CompletedTask;

    #endregion

    #region Core

    internal Task SendAsync(HttpMethod method, Expression<Func<string>> endpointExpr, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendAsync(method, GetEndpoint(endpointExpr),
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendAsync(HttpMethod method, string endpoint, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        RestRequest request = new(RestClient, method, endpoint, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal Task SendJsonAsync(HttpMethod method, Expression<Func<string>> endpointExpr, object payload, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendJsonAsync(method, GetEndpoint(endpointExpr), payload,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendJsonAsync(HttpMethod method, string endpoint, object payload, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        string? json = SerializeJson(payload);
        JsonRestRequest request = new(RestClient, method, endpoint, json, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal Task SendMultipartAsync(HttpMethod method, Expression<Func<string>> endpointExpr,
        IReadOnlyDictionary<string, object> multipartArgs, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendMultipartAsync(method, GetEndpoint(endpointExpr), multipartArgs,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendMultipartAsync(HttpMethod method, string endpoint,
        IReadOnlyDictionary<string, object> multipartArgs, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        MultipartRestRequest request = new(RestClient, method, endpoint, multipartArgs, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal async Task<TResponse> SendAsync<TResponse>(HttpMethod method, Expression<Func<string>> endpointExpr,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null)
        where TResponse : class =>
        await SendAsync<TResponse>(method, GetEndpoint(endpointExpr),
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    internal async Task<TResponse> SendAsync<TResponse, TArg1, TArg2>(HttpMethod method,
        Expression<Func<TArg1, TArg2, string>> endpointExpr, TArg1 arg1, TArg2 arg2,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null)
        where TResponse : class =>
        await SendAsync<TResponse>(method, GetEndpoint(endpointExpr, arg1, arg2),
            GetBucketId(method, ids, endpointExpr, arg1, arg2, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string endpoint,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
        where TResponse : class
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        RestRequest request = new(RestClient, method, endpoint, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    internal async Task<TResponse> SendJsonAsync<TResponse>(HttpMethod method,
        Expression<Func<string>> endpointExpr, object payload,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null)
        where TResponse : class =>
        await SendJsonAsync<TResponse>(method, GetEndpoint(endpointExpr), payload,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendJsonAsync<TResponse>(HttpMethod method, string endpoint, object payload,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
        where TResponse : class
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        string json = SerializeJson(payload);
        JsonRestRequest request = new(RestClient, method, endpoint, json, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    internal Task<TResponse> SendMultipartAsync<TResponse>(HttpMethod method,
        Expression<Func<string>> endpointExpr, IReadOnlyDictionary<string, object> multipartArgs,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null)
        where TResponse : class =>
        SendMultipartAsync<TResponse>(method, GetEndpoint(endpointExpr), multipartArgs,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendMultipartAsync<TResponse>(HttpMethod method,
        string endpoint, IReadOnlyDictionary<string, object> multipartArgs,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
        where TResponse : class
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        MultipartRestRequest request = new(RestClient, method, endpoint, multipartArgs, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    private async Task<Stream> SendInternalAsync(HttpMethod method, string endpoint, RestRequest request)
    {
        if (!request.Options.IgnoreState)
            CheckState();

        request.Options.RetryMode ??= DefaultRetryMode;
        request.Options.RatelimitCallback ??= DefaultRatelimitCallback;

        Stopwatch stopwatch = Stopwatch.StartNew();
        Stream responseStream = await RequestQueue.SendAsync(request).ConfigureAwait(false);
        stopwatch.Stop();

        double milliseconds = ToMilliseconds(stopwatch);
        await _sentRequestEvent.InvokeAsync(method, endpoint, milliseconds).ConfigureAwait(false);

        return responseStream;
    }

    private async IAsyncEnumerable<IReadOnlyCollection<TItem>> SendPagedAsync<TResponse, TPaged, TItem>(
        HttpMethod method, Expression<Func<int, int, string>> endpointExpr, int offset, int limit,
        Func<TResponse, TPaged> pagedSelector, Func<TPaged, IEnumerable<TItem>> itemsSelector,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        RequestOptions? options = null)
        where TResponse : class
        where TPaged : PagedResponseBase
    {
        int currentOffset = offset;
        int total = int.MaxValue;

        while (currentOffset < total)
        {
            TResponse? response = await SendAsync<TResponse, int, int>(
                    method, endpointExpr, currentOffset, limit, ids, clientBucket, false, options)
                .ConfigureAwait(false);
            if (response == null)
                yield break;
            TPaged pagedResponse = pagedSelector(response);
            IReadOnlyCollection<TItem> items = [..itemsSelector(pagedResponse)];
            yield return items;
            currentOffset += items.Count;
            total = pagedResponse.Total;
            if (items.Count == 0 || currentOffset >= total)
                yield break;
        }
    }

    #endregion

    #region Rooms

    public IAsyncEnumerable<IReadOnlyCollection<Room>> GetJoinedRoomsAsync(
        int limit = HeyBoxConfig.MaxRoomsPerBatchByDefault, int fromOffset = 0,
        RequestOptions? options = null)
    {
        options = RequestOptions.CreateOrClone(options);
        BucketIds ids = new();
        return SendPagedAsync<GetRoomsResponse, GetRoomsPagedResponse, Room>(HttpMethod.Get,
            (o, l) => $"chatroom/v2/room/joined?offset={o}&limit={l}&{HeyBoxConfig.CommonQueryString}",
            fromOffset, limit, x => x.Rooms, x => x.Rooms, ids, options: options);
    }

    #endregion

    #region Room Roles

    public async Task<GetRoomRolesResponse> GetRoomRolesAsync(ulong roomId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(roomId, 0, nameof(roomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(roomId);
        return await SendAsync<GetRoomRolesResponse>(HttpMethod.Get,
                () => $"chatroom/v2/room_role/roles?room_id={roomId}&{HeyBoxConfig.CommonQueryString}", ids,
                options: options)
            .ConfigureAwait(false);
    }

    public async Task<Role> CreateRoomRoleAsync(CreateRoomRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotNullOrEmpty(args.Name, nameof(args.Name));
        Preconditions.Equal(args.Type, RoleType.Custom, nameof(args.Type));
        Preconditions.NotNullOrWhiteSpace(args.Nonce, nameof(args.Nonce));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        return await SendJsonAsync<Role>(HttpMethod.Post,
                () => $"chatroom/v2/room_role/create?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task<Role> ModifyRoomRoleAsync(ModifyRoomRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.Id, 0, nameof(args.Id));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotNullOrEmpty(args.Name, nameof(args.Name));
        Preconditions.Equal(args.Type, RoleType.Custom, nameof(args.Type));
        Preconditions.NotNullOrWhiteSpace(args.Nonce, nameof(args.Nonce));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        return await SendJsonAsync<Role>(HttpMethod.Post,
                () => $"chatroom/v2/room_role/update?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task DeleteRoomRoleAsync(DeleteRoomRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.RoleId, 0, nameof(args.RoleId));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        await SendJsonAsync<object>(HttpMethod.Post,
                () => $"chatroom/v2/room_role/delete?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task<AddRoleResponse> AddRoleAsync(AddOrRemoveRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.UserId, 0, nameof(args.UserId));
        Preconditions.NotEqual(args.RoleId, 0, nameof(args.RoleId));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        return await SendJsonAsync<AddRoleResponse>(HttpMethod.Post,
                () => $"chatroom/v2/room_role/grant?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task RemoveRoleAsync(AddOrRemoveRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.UserId, 0, nameof(args.UserId));
        Preconditions.NotEqual(args.RoleId, 0, nameof(args.RoleId));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        await SendJsonAsync(HttpMethod.Post,
                () => $"chatroom/v2/room_role/revoke?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Messages

    public async Task<SendChannelMessageResponse> SendChannelMessageAsync(SendChannelMessageParams args,
        RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotEqual(args.ChannelId, 0, nameof(args.ChannelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        return await SendJsonAsync<SendChannelMessageResponse>(HttpMethod.Post,
                () => $"chatroom/v2/channel_msg/send?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task<ModifyChannelMessageResponse> ModifyChannelMessageAsync(ModifyChannelMessageParams args,
        RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotEqual(args.ChannelId, 0, nameof(args.ChannelId));
        Preconditions.NotEqual(args.MessageId, 0, nameof(args.MessageId));

        BucketIds ids = new(args.RoomId, args.ChannelId);
        return await SendJsonAsync<ModifyChannelMessageResponse>(HttpMethod.Post,
                () => $"chatroom/v2/channel_msg/update?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task DeleteChannelMessageAsync(DeleteChannelMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotEqual(args.ChannelId, 0, nameof(args.ChannelId));
        Preconditions.NotEqual(args.MessageId, 0, nameof(args.MessageId));

        BucketIds ids = new(args.RoomId, args.ChannelId);
        await SendJsonAsync(HttpMethod.Post,
                () => $"chatroom/v2/channel_msg/delete?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task<SendUserMessageResponse> SendUserMessageAsync(SendUserMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));

        BucketIds ids = new();
        return await SendJsonAsync<SendUserMessageResponse>(HttpMethod.Post,
                () => $"chatroom/v3/msg/user?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Reactions

    public async Task ReplyReactionAsync(ReplyReactionParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.MessageId, 0, nameof(args.MessageId));
        Preconditions.NotNullOrWhiteSpace(args.Emoji, nameof(args.MessageId));
        Preconditions.NotEqual(args.ChannelId, 0, nameof(args.ChannelId));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId, args.ChannelId);
        await SendJsonAsync<object>(HttpMethod.Post,
                () => $"chatroom/v2/channel_msg/emoji/reply?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Assets

    public async Task<CreateAssetResponse> CreateAssetAsync(CreateAssetParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        return await SendMultipartAsync<CreateAssetResponse>(HttpMethod.Post,
                () => $"{HeyBoxConfig.CreateAssetAPIUrl}upload", args.ToDictionary(), ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Memes

    public async Task<GetRoomMemesResponse> GetRoomMemesAsync(ulong roomId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(roomId, 0, nameof(roomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(roomId);
        return await SendAsync<GetRoomMemesResponse>(HttpMethod.Get,
                () => $"chatroom/v3/msg/meme/room/list?{HeyBoxConfig.CommonQueryString}&room_id={roomId}", ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task DeleteRoomMemeAsync(DeleteRoomMemeParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.Path, 0, nameof(args.Path));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        await SendJsonAsync<object>(HttpMethod.Post,
                () => $"chatroom/v2/msg/meme/room/del?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    public async Task ModifyRoomMemeAsync(ModifyRoomMemeParams args, RequestOptions? options = null)
    {
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotEqual(args.Path, 0, nameof(args.Path));
        Preconditions.NotEqual(args.RoomId, 0, nameof(args.RoomId));
        Preconditions.NotNullOrEmpty(args.Name, nameof(args.Name));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(args.RoomId);
        await SendJsonAsync<object>(HttpMethod.Post,
                () => $"chatroom/v2/msg/meme/room/edit?{HeyBoxConfig.CommonQueryString}", args, ids, options: options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Helpers

    protected void CheckState()
    {
        if (LoginState != LoginState.LoggedIn)
            throw new InvalidOperationException("Client is not logged in.");
    }

    protected static double ToMilliseconds(Stopwatch stopwatch) =>
        Math.Round((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0, 2);

    [return: NotNullIfNotNull(nameof(payload))]
    protected string? SerializeJson(object? payload) =>
        payload is null ? null : JsonSerializer.Serialize(payload, _serializerOptions);

    protected async Task<T> DeserializeJsonAsync<T>(Stream jsonStream)
    {
        try
        {
            T? jsonObject = await JsonSerializer.DeserializeAsync<T>(jsonStream, _serializerOptions).ConfigureAwait(false);
            if (jsonObject is null)
                throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}");
            return jsonObject;
        }
        catch (JsonException ex)
        {
            if (jsonStream is MemoryStream memoryStream)
            {
                string json = Encoding.UTF8.GetString(memoryStream.ToArray());
                throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}\nJSON: {json}", ex);
            }

            throw;
        }
    }

    internal class BucketIds
    {
        public ulong RoomId { get; internal set; }
        public ulong ChannelId { get; internal set; }
        public HttpMethod? HttpMethod { get; internal set; }

        internal BucketIds(ulong roomId = 0, ulong channelId = 0)
        {
            RoomId = roomId;
            ChannelId = channelId;
        }

        internal object?[] ToArray() =>
            [HttpMethod, RoomId, ChannelId];

        internal Dictionary<string, string> ToMajorParametersDictionary()
        {
            Dictionary<string, string> dict = new();
            if (RoomId != 0)
                dict["RoomId"] = RoomId.ToString();
            if (ChannelId != 0)
                dict["ChannelId"] = ChannelId.ToString();
            return dict;
        }

        internal static int? GetIndex(string name) =>
            name switch
            {
                "httpMethod" => 0,
                "roomId" => 1,
                "channelId" => 2,
                _ => null
            };
    }

    private static string GetEndpoint(Expression<Func<string>> endpointExpr) => endpointExpr.Compile()();

    private static string GetEndpoint<T1, T2>(Expression<Func<T1, T2, string>> endpointExpr, T1 arg1, T2 arg2) => endpointExpr.Compile()(arg1, arg2);

    private static BucketId GetBucketId(HttpMethod httpMethod, BucketIds ids, Expression<Func<string>> endpointExpr, string? callingMethod)
    {
        Preconditions.NotNull(callingMethod, nameof(callingMethod));
        ids.HttpMethod = httpMethod;
        return _bucketIdGenerators.GetOrAdd(callingMethod, _ => CreateBucketId(endpointExpr))(ids);
    }

    private static BucketId GetBucketId<TArg1, TArg2>(HttpMethod httpMethod, BucketIds ids, Expression<Func<TArg1, TArg2, string>> endpointExpr,
        TArg1 arg1, TArg2 arg2, string? callingMethod)
    {
        Preconditions.NotNull(callingMethod, nameof(callingMethod));
        ids.HttpMethod = httpMethod;
        return _bucketIdGenerators.GetOrAdd(callingMethod, x => CreateBucketId(endpointExpr, arg1, arg2))(ids);
    }

    private static Func<BucketIds, BucketId> CreateBucketId<TArg1, TArg2>(Expression<Func<TArg1, TArg2, string>> endpoint, TArg1 arg1, TArg2 arg2) =>
        CreateBucketId(() => endpoint.Compile().Invoke(arg1, arg2));

    private static Func<BucketIds, BucketId> CreateBucketId(Expression<Func<string>> endpoint)
    {
        try
        {
            //Is this a constant string?
            if (endpoint.Body.NodeType == ExpressionType.Constant)
                return x => BucketId.Create(x.HttpMethod, GetPathIfPossible((endpoint.Body as ConstantExpression)?.Value?.ToString()), x.ToMajorParametersDictionary());

            StringBuilder builder = new();

            MethodCallExpression methodCall = (MethodCallExpression) endpoint.Body;
            Expression[] methodArgs = methodCall.Arguments.ToArray();
            string format = methodArgs[0].NodeType == ExpressionType.Constant
                ? ((ConstantExpression) methodArgs[0]).Value!.ToString()!
                : endpoint.Compile()();

            //Unpack the array, if one exists (happens with 4+ parameters)
            if (methodArgs.Length > 1 && methodArgs[1].NodeType == ExpressionType.NewArrayInit)
            {
                NewArrayExpression arrayExpr = (NewArrayExpression) methodArgs[1];
                Expression[] elements = arrayExpr.Expressions.ToArray();
                Array.Resize(ref methodArgs, elements.Length + 1);
                Array.Copy(elements, 0, methodArgs, 1, elements.Length);
            }

            int endIndex = format.IndexOf('?'); //Don't include params
            if (endIndex == -1)
                endIndex = format.Length;

            int lastIndex = 0;
            while (true)
            {
                int leftIndex = format.IndexOf("{", lastIndex, StringComparison.Ordinal);
                if (leftIndex == -1 || leftIndex > endIndex)
                {
                    builder.Append(format, lastIndex, endIndex - lastIndex);
                    break;
                }

                builder.Append(format, lastIndex, leftIndex - lastIndex);
                int rightIndex = format.IndexOf("}", leftIndex, StringComparison.Ordinal);

                int argId = int.Parse(format.Substring(leftIndex + 1, rightIndex - leftIndex - 1), NumberStyles.None, CultureInfo.InvariantCulture);
                string fieldName = GetFieldName(methodArgs[argId + 1]);

                int? mappedId = BucketIds.GetIndex(fieldName);

                if (!mappedId.HasValue
                    && rightIndex != endIndex
                    && format.Length > rightIndex + 1
                    && format[rightIndex + 1] == '/') //Ignore the next slash
                    rightIndex++;

                if (mappedId.HasValue)
                    builder.Append($"{{{mappedId.Value}}}");

                lastIndex = rightIndex + 1;
            }

            if (builder[^1] == '/')
                builder.Remove(builder.Length - 1, 1);

            format = builder.ToString();

            return x => BucketId.Create(x.HttpMethod, GetPathIfPossible(string.Format(format, x.ToArray())), x.ToMajorParametersDictionary());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate the bucket id for this operation.", ex);
        }
    }

    private static string? GetPathIfPossible(string? endpoint)
    {
        if (Uri.TryCreate(endpoint, UriKind.Absolute, out Uri? absoluteUri))
            return absoluteUri.PathAndQuery;
        return endpoint;
    }

    private static string GetFieldName(Expression expr)
    {
        if (expr.NodeType == ExpressionType.Convert)
            expr = ((UnaryExpression) expr).Operand;

        if (expr.NodeType != ExpressionType.MemberAccess)
            throw new InvalidOperationException("Unsupported expression");

        return ((MemberExpression) expr).Member.Name;
    }

    #endregion
}
