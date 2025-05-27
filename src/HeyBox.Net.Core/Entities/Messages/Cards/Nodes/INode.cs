namespace HeyBox;

/// <summary>
///     表示一个通用的节点，可用于 <see cref="HeyBox.IModule"/> 中。
/// </summary>
public interface INode
{
    /// <summary>
    ///     获取节点的类型。
    /// </summary>
    NodeType Type { get; }

    /// <summary>
    ///     获取节点的宽度。
    /// </summary>
    NodeWidth? Width { get; }
}
