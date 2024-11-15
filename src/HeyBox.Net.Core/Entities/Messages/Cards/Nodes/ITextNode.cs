namespace HeyBox;

/// <summary>
///     表示一个文本节点。
/// </summary>
public interface ITextNode : INode
{
    /// <summary>
    ///     获取文本节点的文本。
    /// </summary>
    string Text { get; }
}
