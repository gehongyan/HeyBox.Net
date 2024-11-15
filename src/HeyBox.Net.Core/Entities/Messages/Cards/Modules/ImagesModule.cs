using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     图片模块，可用于 <see cref="ICard"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ImagesModule : IModule, IEquatable<ImagesModule>, IEquatable<IModule>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.Images;

    internal ImagesModule(ImmutableArray<ImageNode> images)
    {
        Images = images;
    }

    /// <summary>
    ///     获取图片模块的图片链接。
    /// </summary>
    public ImmutableArray<ImageNode> Images { get; }

    private string DebuggerDisplay => $"{Type} ({Images.Length} Image{(Images.Length == 1 ? string.Empty : "s")})";

    /// <summary>
    ///     判定两个 <see cref="ImagesModule"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImagesModule"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ImagesModule left, ImagesModule right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ImagesModule"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImagesModule"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ImagesModule left, ImagesModule right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ImagesModule imagesModule && Equals(imagesModule);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ImagesModule? imagesModule) =>
        GetHashCode() == imagesModule?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ Type.GetHashCode();
            foreach (ImageNode imageNode in Images)
                hash = (hash * 16777619) ^ imageNode.GetHashCode();
            return hash;
        }
    }

    bool IEquatable<IModule>.Equals([NotNullWhen(true)] IModule? module) =>
        Equals(module as ImagesModule);
}
