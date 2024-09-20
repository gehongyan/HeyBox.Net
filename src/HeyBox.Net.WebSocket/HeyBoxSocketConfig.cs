using HeyBox.Net.Queue;
using HeyBox.Net.Queue.SynchronousImmediate;
using HeyBox.Net.WebSockets;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个用于 <see cref="HeyBox.WebSocket.HeyBoxSocketClient"/> 的配置类。
/// </summary>
/// <remarks>
///     此配置基于 <see cref="HeyBox.Rest.HeyBoxRestConfig"/>，在与 REST 有关的配置的基础上，定义了有关网关的配置。
/// </remarks>
/// <example>
///     以下代码启用了消息缓存，并配置客户端在房间可用时始终下载用户。
///     <code language="cs">
///     var config = new HeyBoxSocketConfig
///     {
///         AlwaysDownloadUsers = true,
///         MessageCacheSize = 100
///     };
///     var client = new HeyBoxSocketClient(config);
///     </code>
/// </example>
public class HeyBoxSocketConfig : HeyBoxRestConfig
{
    /// <summary>
    ///     获取网关使用的数据格式。
    /// </summary>
    public const string GatewayEncoding = "json";

    /// <summary>
    ///     获取或设置要连接的网关地址。
    /// </summary>
    public string? GatewayHost { get; set; }

    /// <summary>
    ///     获取或设置要连接的网关端口。
    /// </summary>
    public const string DefaultGatewayUrl = "wss://chat.xiaoheihe.cn/chatroom/ws/connect";

    /// <summary>
    ///     获取或设置连接到网关时的超时时间间隔（毫秒）。
    /// </summary>
    public int ConnectionTimeout { get; set; } = 6000;

    /// <summary>
    ///     获取网关发送心跳包的时间间隔（毫秒）。
    /// </summary>
    public int HeartbeatIntervalMilliseconds { get; internal set; } = 30000;

    /// <summary>
    ///     获取或设置阻塞网关线程的事件处理程序的超时时间间隔（毫秒），超过此时间间隔的阻塞网关线程的事件处理程序会被日志记录警告。将此属性设置为 <c>null</c> 将禁用此检查。
    /// </summary>
    public int? HandlerTimeout { get; set; } = 3000;

    /// <summary>
    ///     获取或设置用于创建 WebSocket 客户端的委托。
    /// </summary>
    public WebSocketProvider WebSocketProvider { get; set; }

    /// <summary>
    ///     获取或设置用于创建消息队列的委托。
    /// </summary>
    public MessageQueueProvider MessageQueueProvider { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="HeyBoxSocketConfig"/> 类的新实例。
    /// </summary>
    public HeyBoxSocketConfig()
    {
        WebSocketProvider = DefaultWebSocketProvider.Instance;
        MessageQueueProvider = SynchronousImmediateMessageQueueProvider.Instance;
    }

    internal HeyBoxSocketConfig Clone() => (HeyBoxSocketConfig)MemberwiseClone();
}
