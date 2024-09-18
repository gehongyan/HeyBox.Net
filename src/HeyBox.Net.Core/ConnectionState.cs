namespace HeyBox;

/// <summary>
///     指定客户端的连接状态。
/// </summary>
public enum ConnectionState : byte
{
    /// <summary>
    ///     客户端已断开与黑盒语音的连接。
    /// </summary>
    Disconnected,

    /// <summary>
    ///     客户端正在连接到黑盒语音。
    /// </summary>
    Connecting,

    /// <summary>
    ///     客户端已连接到黑盒语音。
    /// </summary>
    Connected,

    /// <summary>
    ///     客户端正在断开与黑盒语音的连接。
    /// </summary>
    Disconnecting
}
