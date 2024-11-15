using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="MarkdownNode"/> 元素的构建器。
/// </summary>
public class MarkdownNodeBuilder : ITextNodeBuilder, IEquatable<MarkdownNodeBuilder>, IEquatable<INodeBuilder>
{
    /// <summary>
    ///     Markdown 文本的最大长度。
    /// </summary>
    public const int MaxMarkdownLength = 10000;

    /// <summary>
    ///     初始化一个 <see cref="MarkdownNodeBuilder"/> 类的新实例。
    /// </summary>
    public MarkdownNodeBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownNodeBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> Markdown 文本内容。 </param>
    public MarkdownNodeBuilder(string? text)
    {
        Text = text;
    }

    /// <inheritdoc />
    public NodeType Type => NodeType.Markdown;

    /// <inheritdoc />
    public string? Text { get; set; }

    /// <summary>
    ///     设置 Markdown 的文本内容，值将被设置到 <see cref="Text"/> 属性上。
    /// </summary>
    /// <param name="text"> Markdown 的文本内容。 </param>
    /// <returns> 当前构建器。 </returns>
    public MarkdownNodeBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="MarkdownNode"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="MarkdownNode"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Text"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Text"/> 的长度超过了 <see cref="MaxMarkdownLength"/>。
    /// </exception>
    [MemberNotNull(nameof(Text))]
    public MarkdownNode Build()
    {
        if (Text == null)
            throw new ArgumentNullException(nameof(Text), $"The {nameof(Text)} cannot be null.");

        if (Text.Length > MaxMarkdownLength)
            throw new ArgumentException(
                $"Markdown length must be less than or equal to {MaxMarkdownLength}.",
                nameof(Text));

        return new MarkdownNode(Text);
    }

    /// <summary>
    ///     使用指定的 Markdown 文本内容初始化一个新的 <see cref="MarkdownNodeBuilder"/> 类的实例。
    /// </summary>
    /// <param name="content"> Markdown 文本内容。 </param>
    /// <returns> 一个使用指定的 Markdown 文本内容初始化的 <see cref="MarkdownNodeBuilder"/> 类的实例。 </returns>
    public static implicit operator MarkdownNodeBuilder(string content) => new(content);

    /// <inheritdoc />
    [MemberNotNull(nameof(Text))]
    INode INodeBuilder.Build() => Build();

    /// <inheritdoc />
    [MemberNotNull(nameof(Text))]
    ITextNode ITextNodeBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="MarkdownNodeBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="MarkdownNodeBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownNodeBuilder? left, MarkdownNodeBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="MarkdownNodeBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="MarkdownNodeBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownNodeBuilder? left, MarkdownNodeBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is MarkdownNodeBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] MarkdownNodeBuilder? markdownNodeBuilder)
    {
        if (markdownNodeBuilder is null)
            return false;
        return Type == markdownNodeBuilder.Type
            && Text == markdownNodeBuilder.Text;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<INodeBuilder>.Equals([NotNullWhen(true)] INodeBuilder? nodeBuilder) =>
        Equals(nodeBuilder as MarkdownNodeBuilder);

    /// <inheritdoc />
    INode INodeBuilder.Build(IModuleBuilder parent) => Build();
}
