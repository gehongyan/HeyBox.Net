namespace HeyBox;

/// <summary>
///     表示一个通用的文本节点构建器，用于构建 <see cref="ITextNode"/> 节点。
/// </summary>
public interface ITextNodeBuilder : INodeBuilder
{
    /// <summary>
    ///     获取或设置 Markdown 的文本内容。
    /// </summary>
    string? Text { get; set; }

    /// <summary>
    ///     设置文本节点的文本。
    /// </summary>
    /// <returns></returns>
    new ITextNode Build();

    /// <inheritdoc />
    INode INodeBuilder.Build() => Build();
}
