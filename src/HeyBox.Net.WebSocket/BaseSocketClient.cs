using HeyBox.API;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的客户端的抽象基类。
/// </summary>
public abstract class BaseSocketClient : BaseHeyBoxClient, IHeyBoxClient
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
    ///     获取一个房间。
    /// </summary>
    /// <param name="id"> 要获取的房间的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的实体；如果缓存中不存在指定的房间，则返回一个用于后续调用但不包含其他有效信息的房间实体。 </returns>
    public abstract SocketRoom GetRoom(ulong id);

    /// <inheritdoc />
    public abstract Task StartAsync();

    /// <inheritdoc />
    public abstract Task StopAsync();
}
