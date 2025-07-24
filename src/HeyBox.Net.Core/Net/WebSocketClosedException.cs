namespace HeyBox.Net;

/// <summary>
///     表示一个由黑盒语音意外关闭 WebSocket 会话时引发的异常。
/// </summary>
public class WebSocketClosedException : Exception
{
    /// <summary>
    ///     获取由黑盒语音发送的关闭代码。
    /// </summary>
    public int? CloseCode { get; }

    /// <summary>
    ///     获取中断的原因。
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    ///     使用黑盒语音发送的关闭代码及原因初始化一个 <see cref="WebSocketClosedException" /> 类的新实例。
    /// </summary>
    /// <param name="closeCode"> 由黑盒语音发送的关闭代码。 </param>
    /// <param name="reason"> 中断的原因。 </param>
    public WebSocketClosedException(int? closeCode, string? reason = null)
        : base($"The server sent close {closeCode}{(reason != null ? $": \"{reason}\"" : "")}")
    {
        CloseCode = closeCode;
        Reason = reason;
    }
}
