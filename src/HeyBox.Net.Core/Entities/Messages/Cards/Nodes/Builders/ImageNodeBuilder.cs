using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="ImageNode"/> 元素的构建器。
/// </summary>
public class ImageNodeBuilder : INodeBuilder, IEquatable<ImageNodeBuilder>, IEquatable<INodeBuilder>
{
    /// <summary>
    ///     初始化一个 <see cref="ImageNodeBuilder"/> 类的新实例。
    /// </summary>
    public ImageNodeBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="ImageNodeBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="url"> 图片的源。 </param>
    /// <param name="size"> 图片的大小。 </param>
    /// <remarks>
    ///     <paramref name="size"/> 仅在 <see cref="SectionModuleBuilder"/> 中生效且必需，<see cref="ImagesModuleBuilder"/> 中不生效。
    /// </remarks>
    public ImageNodeBuilder(string url, ImageSize? size = null)
    {
        Url = url;
        Size = size;
    }

    /// <inheritdoc />
    public NodeType Type => NodeType.Image;

    /// <summary>
    ///     获取或设置图片的源。
    /// </summary>
    /// <remarks>
    ///     图片的媒体类型仅支持 <c>image/jpeg</c>、<c>image/gif</c>、<c>image/png</c>。
    /// </remarks>
    public string? Url { get; set; }

    /// <summary>
    ///     获取或设置 <see cref="ImageNodeBuilder"/> 的图片大小。
    /// </summary>
    /// <remarks>
    ///     当前属性仅在 <see cref="SectionModuleBuilder"/> 中生效且必需，<see cref="ImagesModuleBuilder"/> 中不生效。
    /// </remarks>
    public ImageSize? Size { get; set; }

    /// <summary>
    ///     设置图片的源，值将被设置到 <see cref="Url"/> 属性上。
    /// </summary>
    /// <param name="url"> 图片的源。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <remarks>
    ///     图片的媒体类型仅支持 <c>image/jpeg</c>、<c>image/gif</c>、<c>image/png</c>。
    /// </remarks>
    public ImageNodeBuilder WithUrl(string? url)
    {
        Url = url;
        return this;
    }

    /// <summary>
    ///     设置图片的大小，值将被设置到 <see cref="Size"/> 属性上。
    /// </summary>
    /// <param name="size"> 图片的大小。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <remarks>
    ///     图片的尺寸仅在 <see cref="SectionModuleBuilder"/> 中生效且必需，<see cref="ImagesModuleBuilder"/> 中不生效。
    /// </remarks>
    public ImageNodeBuilder WithSize(ImageSize? size)
    {
        Size = size;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="ImageNode"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="ImageNode"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Url"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Url"/> 为空字符串。
    /// </exception>
    /// <exception cref="UriFormatException">
    ///     <see cref="Url"/> 不是有效的 URL。
    /// </exception>
    [MemberNotNull(nameof(Url))]
    public ImageNode Build()
    {
        if (Url == null)
            throw new ArgumentNullException(nameof(Url), "The source url cannot be null or empty.");
        if (string.IsNullOrEmpty(Url))
            throw new ArgumentException("The source url cannot be null or empty.", nameof(Url));

        UrlValidation.Validate(Url);

        return new ImageNode(Url, Size);
    }

    internal ImageNode Build(IModuleBuilder parent)
    {
        if (parent is SectionModuleBuilder && !Size.HasValue)
            throw new InvalidOperationException("The image size is required in a SectionModule.");
        return Build();
    }

    /// <summary>
    ///     使用指定的图片源初始化一个新的 <see cref="ImageNodeBuilder"/> 类的实例。
    /// </summary>
    /// <param name="source"> 图片的源。 </param>
    /// <returns> 一个使用指定的图片源初始化的 <see cref="ImageNodeBuilder"/> 类的实例。 </returns>
    public static implicit operator ImageNodeBuilder(string source) => new(source);

    /// <inheritdoc />
    [MemberNotNull(nameof(Url))]
    INode INodeBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="ImageNodeBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImageNodeBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ImageNodeBuilder? left, ImageNodeBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ImageNodeBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImageNodeBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ImageNodeBuilder? left, ImageNodeBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ImageNodeBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ImageNodeBuilder? imageNodeBuilder)
    {
        if (imageNodeBuilder is null) return false;

        return Type == imageNodeBuilder.Type
            && Url == imageNodeBuilder.Url
            && Size == imageNodeBuilder.Size;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<INodeBuilder>.Equals([NotNullWhen(true)] INodeBuilder? nodeBuilder) =>
        Equals(nodeBuilder as ImageNodeBuilder);

    /// <inheritdoc />
    INode INodeBuilder.Build(IModuleBuilder parent) => Build(parent);
}
