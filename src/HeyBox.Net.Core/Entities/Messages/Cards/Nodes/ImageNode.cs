using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     图片节点，可用于 <see cref="HeyBox.IModule"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ImageNode : INode, IEquatable<ImageNode>, IEquatable<INode>
{
    /// <inheritdoc />
    public NodeType Type => NodeType.Image;

    internal ImageNode(string url, ImageSize? size, NodeWidth? width)
    {
        Url = url;
        Size = size;
        Width = width;
    }

    /// <summary>
    ///     获取图片的 URL。
    /// </summary>
    public string Url { get; }

    /// <inheritdoc />
    public NodeWidth? Width { get; }

    /// <summary>
    ///     获取图片的尺寸。
    /// </summary>
    /// <remarks>
    ///     图片的尺寸仅定义在 <see cref="SectionModule"/> 中，<see cref="ImagesModule"/> 中此属性为 <c>null</c>。
    /// </remarks>
    public ImageSize? Size { get; }

    private string DebuggerDisplay => $"{Type}: {Url}";

    /// <summary>
    ///     判定两个 <see cref="ImageNode"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImageNode"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ImageNode? left, ImageNode? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ImageNode"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImageNode"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ImageNode? left, ImageNode? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ImageNode imageNode && Equals(imageNode);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ImageNode? imageNode) =>
        GetHashCode() == imageNode?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Type, Url, Size);

    bool IEquatable<INode>.Equals([NotNullWhen(true)] INode? node) =>
        Equals(node as ImageNode);
}
