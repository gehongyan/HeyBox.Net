namespace HeyBox;

/// <summary>
///     表示一个节点的类型。
/// </summary>
public enum NodeType
{
    /// <summary>
    ///     文本节点。
    /// </summary>
    PlainText,

    /// <summary>
    ///     Markdown 节点。
    /// </summary>
    Markdown,

    /// <summary>
    ///     按钮节点。
    /// </summary>
    Button,

    /// <summary>
    ///     图片节点。
    /// </summary>
    Image
}
