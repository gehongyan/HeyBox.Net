using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     Markdown 文本节点，可用于 <see cref="HeyBox.IModule"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MarkdownNode : ITextNode, IEquatable<MarkdownNode>, IEquatable<INode>
{
    /// <inheritdoc />
    public NodeType Type => NodeType.Markdown;

    internal MarkdownNode(string text)
    {
        Text = text;
    }

    /// <inheritdoc />
    public string Text { get; }

    /// <inheritdoc />
    public override string ToString() => Text;

    private string DebuggerDisplay => $"{Type}: {Text}";

    /// <summary>
    ///     判定两个 <see cref="MarkdownNode"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="MarkdownNode"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownNode? left, MarkdownNode? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="MarkdownNode"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="MarkdownNode"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownNode? left, MarkdownNode? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MarkdownNode markdownNode && Equals(markdownNode);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] MarkdownNode? markdownNode) =>
        GetHashCode() == markdownNode?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Type, Text);

    bool IEquatable<INode>.Equals([NotNullWhen(true)] INode? node) =>
        Equals(node as MarkdownNode);
}
