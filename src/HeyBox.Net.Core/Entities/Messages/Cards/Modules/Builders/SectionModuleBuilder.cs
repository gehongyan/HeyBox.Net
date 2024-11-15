using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="SectionModule"/> 模块的构建器。
/// </summary>
public class SectionModuleBuilder : IModuleBuilder, IEquatable<SectionModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <summary>
    ///     获取模块的最大段落数量。
    /// </summary>
    public const int MaxParagraphCount = 3;

    /// <inheritdoc />
    public ModuleType Type => ModuleType.Section;

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    public SectionModuleBuilder()
    {
        Paragraph = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="paragraph"> 段落元素。 </param>
    public SectionModuleBuilder(IList<INodeBuilder> paragraph)
    {
        Paragraph = paragraph;
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="firstText"> 位于段落左侧的文本节点构建器。 </param>
    public SectionModuleBuilder(ITextNodeBuilder firstText)
    {
        Paragraph = [firstText];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="firstText"> 位于段落左侧的文本节点构建器。 </param>
    /// <param name="secondText"> 位于段落中部的文本节点构建器。 </param>
    public SectionModuleBuilder(ITextNodeBuilder firstText, ITextNodeBuilder secondText)
    {
        Paragraph = [firstText, secondText];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="firstText"> 位于段落左侧的文本节点构建器。 </param>
    /// <param name="secondText"> 位于段落中部的文本节点构建器。 </param>
    /// <param name="thirdText"> 位于段落右侧的文本节点构建器。 </param>
    public SectionModuleBuilder(ITextNodeBuilder firstText, ITextNodeBuilder secondText, ITextNodeBuilder thirdText)
    {
        Paragraph = [firstText, secondText, thirdText];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 位于段落左侧的文本节点构建器。 </param>
    /// <param name="button"> 位于段落右侧的按钮节点构建器。 </param>
    public SectionModuleBuilder(ITextNodeBuilder text, ButtonNodeBuilder button)
    {
        Paragraph = [text, button];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 位于段落左侧的文本节点构建器。 </param>
    /// <param name="image"> 位于段落右侧的图片节点构建器。 </param>
    public SectionModuleBuilder(ITextNodeBuilder text, ImageNodeBuilder image)
    {
        Paragraph = [text, image];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 位于段落左侧的图片节点构建器。 </param>
    /// <param name="image"> 位于段落右侧的文本节点构建器。 </param>
    public SectionModuleBuilder(ImageNodeBuilder image, ITextNodeBuilder text)
    {
        Paragraph = [image, text];
    }

    /// <summary>
    ///     初始化一个 <see cref="SectionModuleBuilder"/> 类的新实例。
    /// </summary>
    public IList<INodeBuilder> Paragraph { get; }

    /// <summary>
    ///     添加一个元素到文本模块。
    /// </summary>
    /// <param name="field"> 要添加的按钮元素。 </param>
    /// <returns> 当前构建器。 </returns>
    public SectionModuleBuilder AddNode(INodeBuilder field)
    {
        Paragraph.Add(field);
        return this;
    }

    /// <summary>
    ///     添加一个元素到文本模块。
    /// </summary>
    /// <param name="button"> 一个包含对要添加的新创建的元素的配置的操作。 </param>
    /// <returns> 当前构建器。 </returns>
    public SectionModuleBuilder AddNode<T>(Action<T>? button = null)
        where T : INodeBuilder, new()
    {
        T field = new();
        button?.Invoke(field);
        Paragraph.Add(field);
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="SectionModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="SectionModule"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Paragraph"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Paragraph"/> 是一个空列表。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Paragraph"/> 的元素数量超过了 <see cref="MaxParagraphCount"/>。
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     <see cref="Paragraph"/> 的元素不符合模块的规范。支持的模式有：<br/>
    ///     <c>[文本]</c> 或 <c>[文本, 文本/按钮/图片]</c> 或 <c>[文本, 文本, 文本]</c> 或 <c>[图片, 文本]</c>。
    /// </exception>
    public SectionModule Build()
    {
        if (Paragraph == null)
            throw new ArgumentNullException(
                nameof(Paragraph), "Paragraph cannot be null or empty list.");

        if (Paragraph.Count == 0)
            throw new ArgumentException(
                "Paragraph cannot be null or empty list.", nameof(Paragraph));

        if (Paragraph.Count > MaxParagraphCount)
            throw new ArgumentException(
                $"Paragraph count must be less than or equal to {MaxParagraphCount}.", nameof(Paragraph));

        if (Paragraph is not (
            [ITextNodeBuilder]
            or [ITextNodeBuilder, ITextNodeBuilder or ButtonNodeBuilder or ImageNodeBuilder]
            or [ITextNodeBuilder, ITextNodeBuilder, ITextNodeBuilder]
            or [ImageNodeBuilder, ITextNodeBuilder]))
            throw new InvalidOperationException("""
                The paragraph schema of the section module is invalid. Valid schemas are:
                - [Text]
                - [Text, Text or Button or Image]
                - [Text, Text, Text]
                - [Image, Text]
                """);

        return new SectionModule([..Paragraph.Select(x => x.Build(this))]);
    }

    /// <inheritdoc />
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="SectionModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="SectionModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(SectionModuleBuilder? left, SectionModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="SectionModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="SectionModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(SectionModuleBuilder? left, SectionModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is SectionModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] SectionModuleBuilder? sectionModuleBuilder)
    {
        if (sectionModuleBuilder is null) return false;

        if (Paragraph.Count != sectionModuleBuilder.Paragraph.Count)
            return false;

        if (Paragraph
            .Zip(sectionModuleBuilder.Paragraph, (x, y) => (x, y))
            .Any(pair => pair.x != pair.y))
            return false;

        return Type == sectionModuleBuilder.Type;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as SectionModuleBuilder);
}
