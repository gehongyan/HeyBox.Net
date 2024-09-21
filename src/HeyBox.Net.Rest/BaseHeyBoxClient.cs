using HeyBox.Logging;

namespace HeyBox.Rest;

/// <summary>
///     表示一个可以连接到 HeyBox API 的通用的 HeyBox Bot 客户端。
/// </summary>
public abstract class BaseHeyBoxClient : IHeyBoxClient
{
    #region BaseHeyBoxClient

    /// <summary>
    ///     当生成一条日志消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.LogMessage"/> 参数是描述日志消息的结构。 </item>
    ///     </list>
    /// </remarks>
    public event Func<LogMessage, Task> Log
    {
        add => _logEvent.Add(value);
        remove => _logEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<LogMessage, Task>> _logEvent = new();

    /// <summary>
    ///     当客户端登录成功时引发。
    /// </summary>
    public event Func<Task> LoggedIn
    {
        add => _loggedInEvent.Add(value);
        remove => _loggedInEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _loggedInEvent = new();

    /// <summary>
    ///     当客户端退出登录时引发。
    /// </summary>
    public event Func<Task> LoggedOut
    {
        add => _loggedOutEvent.Add(value);
        remove => _loggedOutEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _loggedOutEvent = new();

    /// <summary>
    ///     当向 API 发送 REST 请求时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="System.Net.Http.HttpMethod"/> 参数是 HTTP 方法。 </item>
    ///     <item> <see cref="System.String"/> 参数是终结点。 </item>
    ///     <item> <see cref="System.Double"/> 参数是完成请求所花费的时间，以毫秒为单位。 </item>
    ///     </list>
    /// </remarks>
    public event Func<HttpMethod, string, double, Task> SentRequest
    {
        add => _sentRequest.Add(value);
        remove => _sentRequest.Remove(value);
    }

    internal readonly AsyncEvent<Func<HttpMethod, string, double, Task>> _sentRequest = new();

    internal readonly Logger _restLogger;
    private readonly SemaphoreSlim _stateLock;
    private bool _isFirstLogin, _isDisposed;

    internal API.HeyBoxRestApiClient ApiClient { get; }

    internal LogManager LogManager { get; }

    /// <summary>
    ///     获取此客户端的登录状态。
    /// </summary>
    public LoginState LoginState { get; protected set; }

    /// <summary>
    ///     获取登录到此客户端的当前用户；如果未登录，则为 <c>null</c>。
    /// </summary>
    public ISelfUser? CurrentUser { get; protected set; }

    /// <inheritdoc />
    public TokenType TokenType => ApiClient.AuthTokenType;

    internal string? Token => ApiClient.AuthToken;

    internal BaseHeyBoxClient(HeyBoxRestConfig config, API.HeyBoxRestApiClient client)
    {
        ApiClient = client;
        LogManager = new LogManager(config.LogLevel);
        LogManager.Message += async msg => await _logEvent.InvokeAsync(msg).ConfigureAwait(false);

        _stateLock = new SemaphoreSlim(1, 1);
        _restLogger = LogManager.CreateLogger("Rest");
        _isFirstLogin = config.DisplayInitialLog;

        ApiClient.RequestQueue.RateLimitTriggered += async (id, info, endpoint) =>
        {
            if (info == null)
                await _restLogger.VerboseAsync($"Preemptive Rate limit triggered: {endpoint} {(id.IsHashBucket ? $"(Bucket: {id.BucketHash})" : "")}").ConfigureAwait(false);
            else
                await _restLogger.WarningAsync($"Rate limit triggered: {endpoint} {(id.IsHashBucket ? $"(Bucket: {id.BucketHash})" : "")}").ConfigureAwait(false);
        };
        ApiClient.SentRequest += async (method, endpoint, millis) =>
            await _restLogger.VerboseAsync($"{method} {endpoint}: {millis} ms").ConfigureAwait(false);
        ApiClient.SentRequest += (method, endpoint, millis) =>
            _sentRequest.InvokeAsync(method, endpoint, millis);
    }

    internal virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
        }
    }

    /// <inheritdoc />
    public void Dispose() => Dispose(true);

    /// <inheritdoc />
    public async Task LoginAsync(TokenType tokenType, string token, bool validateToken = true)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LoginInternalAsync(tokenType, token, validateToken).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal virtual async Task LoginInternalAsync(TokenType tokenType, string token, bool validateToken)
    {
        if (_isFirstLogin)
        {
            _isFirstLogin = false;
            await LogManager.WriteInitialLog().ConfigureAwait(false);
        }

        if (LoginState != LoginState.LoggedOut)
            await LogoutInternalAsync().ConfigureAwait(false);

        LoginState = LoginState.LoggingIn;

        try
        {
            // If token validation is enabled, validate the token and let it throw any ArgumentExceptions
            // that result from invalid parameters
            if (validateToken)
            {
                try
                {
                    TokenUtils.ValidateToken(tokenType, token);
                }
                catch (ArgumentException ex)
                {
                    // log these ArgumentExceptions and allow for the client to attempt to log in anyways
                    await LogManager.WarningAsync("HeyBox", "A supplied token was invalid.", ex).ConfigureAwait(false);
                }
            }

            await ApiClient.LoginAsync(tokenType, token).ConfigureAwait(false);
            await OnLoginAsync(tokenType, token).ConfigureAwait(false);
            LoginState = LoginState.LoggedIn;
        }
        catch
        {
            await LogoutInternalAsync().ConfigureAwait(false);
            throw;
        }

        await _loggedInEvent.InvokeAsync().ConfigureAwait(false);
    }

    internal virtual Task OnLoginAsync(TokenType tokenType, string token) => Task.CompletedTask;

    /// <inheritdoc />
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

    internal virtual async Task LogoutInternalAsync()
    {
        if (LoginState == LoginState.LoggedOut)
            return;
        LoginState = LoginState.LoggingOut;
        await ApiClient.LogoutAsync().ConfigureAwait(false);
        await OnLogoutAsync().ConfigureAwait(false);
        LoginState = LoginState.LoggedOut;
        await _loggedOutEvent.InvokeAsync().ConfigureAwait(false);
    }

    internal virtual Task OnLogoutAsync() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual ConnectionState ConnectionState => ConnectionState.Disconnected;

    #endregion

    #region IHeyBoxClient

    // /// <inheritdoc />
    // ISelfUser? IHeyBoxClient.CurrentUser => CurrentUser;

    /// <inheritdoc />
    Task<IRoom?> IHeyBoxClient.GetRoomAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IRoom?>(null);

    /// <inheritdoc />
    Task<IChannel?> IHeyBoxClient.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IChannel?>(null);

    /// <inheritdoc />
    Task<IUser?> IHeyBoxClient.GetUserAsync(uint id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(null);

    /// <inheritdoc />
    Task IHeyBoxClient.StartAsync() =>
        Task.CompletedTask;

    /// <inheritdoc />
    Task IHeyBoxClient.StopAsync() =>
        Task.CompletedTask;

    #endregion
}
