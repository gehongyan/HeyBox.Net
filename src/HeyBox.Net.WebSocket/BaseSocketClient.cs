using HeyBox.API;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的客户端的抽象基类。
/// </summary>
public abstract partial class BaseSocketClient : BaseHeyBoxClient, IHeyBoxClient
{
    /// <summary>
    ///     获取此客户端使用的配置。
    /// </summary>
    protected readonly HeyBoxSocketConfig BaseConfig;

    /// <summary>
    ///     获取到网关服务器的往返延迟估计值（以毫秒为单位）。
    /// </summary>
    /// <remarks>
    ///     此往返估计值源于心跳包的延迟，不代表诸如发送消息等操作的真实延迟。
    /// </remarks>
    public abstract int Latency { get; protected set; }

    /// <summary>
    ///     获取一个与此客户端共享状态的仅限于 REST 的客户端。
    /// </summary>
    public abstract HeyBoxSocketRestClient Rest { get; }

    internal new HeyBoxSocketApiClient ApiClient => base.ApiClient as HeyBoxSocketApiClient
        ?? throw new InvalidOperationException("The API client is not a WebSocket-based client.");

    /// <summary>
    ///     获取当前用户所在的所有房间。
    /// </summary>
    public abstract IReadOnlyCollection<SocketRoom> Rooms { get; }

    /// <inheritdoc cref="HeyBox.Rest.BaseHeyBoxClient.CurrentUser" />
    public new virtual SocketSelfUser? CurrentUser
    {
        get => base.CurrentUser as SocketSelfUser;
        protected set => base.CurrentUser = value;
    }

    internal BaseSocketClient(HeyBoxSocketConfig config, HeyBoxRestApiClient client)
        : base(config, client)
    {
        BaseConfig = config;
    }

    /// <summary>
    ///     获取用户。
    /// </summary>
    /// <remarks>
    ///     此方法可能返回 <c>null</c>，因为此方法仅会返回网关缓存中存在的用户，如果在当前 Bot
    ///     登录会话中，要获取的用户未引发过任何事件，那么该用户实体则不会存在于缓存中。
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public abstract SocketUser? GetUser(uint id);

    /// <summary>
    ///     获取一个服务器频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    public abstract SocketChannel? GetChannel(ulong id);

    /// <summary>
    ///     获取一个私聊频道。
    /// </summary>
    /// <param name="userId"> 私聊频道中另一位用户的 ID。 </param>
    /// <returns> 另一位用户具有指定用户 ID 的私聊频道；如果未找到，则返回 <c>null</c>。 </returns>
    public abstract SocketDMChannel? GetDMChannel(uint userId);

    /// <summary>
    ///     获取一个房间。
    /// </summary>
    /// <param name="id"> 要获取的房间的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的实体；如果缓存中不存在指定的房间，则返回一个用于后续调用但不包含其他有效信息的房间实体。 </returns>
    public abstract SocketRoom? GetRoom(ulong id);

    /// <inheritdoc />
    public abstract Task StartAsync();

    /// <inheritdoc />
    public abstract Task StopAsync();

    #region IHeyBoxClient

    /// <inheritdoc />
    Task<IChannel?> IHeyBoxClient.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IChannel?>(GetChannel(id));

    Task<IDMChannel?> IHeyBoxClient.GetDMChannelAsync(uint userId, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IDMChannel?>(GetDMChannel(userId));

    /// <inheritdoc />
    Task<IUser?> IHeyBoxClient.GetUserAsync(uint id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(GetUser(id));

    #endregion
}
