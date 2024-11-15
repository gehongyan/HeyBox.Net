namespace HeyBox;

/// <summary>
///     表示一个按钮点击交互的数据。
/// </summary>
public interface IButtonClickInteractionData : IHeyBoxInteractionData
{
    /// <summary>
    ///     获取按钮的文本。
    /// </summary>
    string Text { get; }

    /// <summary>
    ///     获取按钮点击后触发的事件类型。
    /// </summary>
    ButtonEvent Event { get; }

    /// <summary>
    ///     获取按钮的值。
    /// </summary>
    string Value { get; }
}
