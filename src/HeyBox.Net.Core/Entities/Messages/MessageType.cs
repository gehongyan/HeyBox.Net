namespace HeyBox;

/// <summary>
///     表示消息的类型。
/// </summary>
public enum MessageType
{
    /// <summary>
    ///     表示文本消息。
    /// </summary>
    Image = 3,

    /// <summary>
    ///     表示 Markdown 消息。
    /// </summary>
    Markdown = 4,

    /// <summary>
    ///     表示带有提及成员的 Markdown 消息。
    /// </summary>
    MarkdownWithMention = 10,

    /// <summary>
    ///     表示卡片消息。
    /// </summary>
    Card = 20
}
