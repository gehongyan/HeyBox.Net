namespace HeyBox;

/// <summary>
///     表示 <see cref="ButtonNode"/> 被点击时触发的事件类型。
/// </summary>
public enum ButtonEvent
{
    /// <summary>
    ///     用户点击按钮时将会被重定向到指定的 URL。
    /// </summary>
    LinkTo,

    /// <summary>
    ///     用户点击按钮时将会提交按钮的 <see cref="HeyBox.ButtonNode.Value"/> 属性的值，黑盒语音将会通过网关携带此值下发事件。
    /// </summary>
    Server
}
