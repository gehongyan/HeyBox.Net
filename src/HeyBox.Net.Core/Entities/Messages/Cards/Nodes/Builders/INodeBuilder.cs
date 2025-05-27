namespace HeyBox;

/// <summary>
///     表示一个通用的节点构建器，用于构建一个 <see cref="INode"/>。
/// </summary>
public interface INodeBuilder
{
    /// <summary>
    ///     获取此构建器构建的节点的类型。
    /// </summary>
    NodeType Type { get; }

    /// <summary>
    ///     获取或设置此构建器构建的节点的大小。
    /// </summary>
    NodeWidth? Width { get; set; }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="INode"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="INode"/> 对象。 </returns>
    INode Build();

    internal INode Build(IModuleBuilder parent);
}
