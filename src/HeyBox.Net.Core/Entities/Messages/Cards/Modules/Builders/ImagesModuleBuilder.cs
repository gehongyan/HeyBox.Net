using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="ImagesModule"/> 模块的构建器。
/// </summary>
public class ImagesModuleBuilder : IModuleBuilder, IEquatable<ImagesModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <summary>
    ///     按钮的最大数量。
    /// </summary>
    public const int MaxImageCount = 18;

    /// <summary>
    ///     初始化一个 <see cref="ImagesModuleBuilder"/> 类的新实例。
    /// </summary>
    public ImagesModuleBuilder()
    {
        Images = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="ImagesModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="images"> 按钮组模块要包含的按钮元素。 </param>
    public ImagesModuleBuilder(IList<ImageNodeBuilder> images)
    {
        Images = images;
    }

    /// <inheritdoc />
    public ModuleType Type => ModuleType.Images;

    /// <summary>
    ///     获取或设置按钮组模块的按钮元素。
    /// </summary>
    public IList<ImageNodeBuilder> Images { get; set; }

    /// <summary>
    ///     添加一个按钮元素到按钮组模块。
    /// </summary>
    /// <param name="field"> 要添加的按钮元素。 </param>
    /// <returns> 当前构建器。 </returns>
    public ImagesModuleBuilder AddImage(ImageNodeBuilder field)
    {
        Images.Add(field);
        return this;
    }

    /// <summary>
    ///     添加一个按钮元素到按钮组模块。
    /// </summary>
    /// <param name="image"> 一个包含对要添加的新创建的按钮元素进行配置的操作的委托。 </param>
    /// <returns> 当前构建器。 </returns>
    public ImagesModuleBuilder AddImage(Action<ImageNodeBuilder>? image = null)
    {
        ImageNodeBuilder field = new();
        image?.Invoke(field);
        Images.Add(field);
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="ImagesModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="ImagesModule"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Images"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Images"/> 是一个空列表。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Images"/> 的元素数量超过了 <see cref="MaxImageCount"/>。
    /// </exception>
    public ImagesModule Build()
    {
        if (Images == null)
            throw new ArgumentNullException(
                nameof(Images), "Images cannot be null or empty list.");
        if (Images.Count == 0)
            throw new ArgumentException(
                "Images cannot be null or empty list.", nameof(Images));
        if (Images.Count > MaxImageCount)
            throw new ArgumentException(
                $"Images count must be less than or equal to {MaxImageCount}.", nameof(Images));
        return new ImagesModule([..Images.Select(e => e.Build(this))]);
    }

    /// <inheritdoc />
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="ImagesModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImagesModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ImagesModuleBuilder? left, ImagesModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ImagesModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ImagesModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ImagesModuleBuilder? left, ImagesModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ImagesModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ImagesModuleBuilder? imagesModuleBuilder)
    {
        if (imagesModuleBuilder is null)
            return false;

        if (Images.Count != imagesModuleBuilder.Images.Count)
            return false;

        if (Images
            .Zip(imagesModuleBuilder.Images, (x, y) => (x, y))
            .Any(pair => pair.x != pair.y))
            return false;

        return Type == imagesModuleBuilder.Type;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as ImagesModuleBuilder);
}
