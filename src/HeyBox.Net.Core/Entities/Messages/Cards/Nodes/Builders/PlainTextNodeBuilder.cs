using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="PlainTextNode"/> 元素的构建器。
/// </summary>
public class PlainTextNodeBuilder : ITextNodeBuilder, IEquatable<PlainTextNodeBuilder>, IEquatable<INodeBuilder>
{
    /// <summary>
    ///     纯文本文本的最大长度。
    /// </summary>
    public const int MaxPlainTextLength = 10000;

    /// <summary>
    ///     初始化一个 <see cref="PlainTextNodeBuilder"/> 类的新实例。
    /// </summary>
    public PlainTextNodeBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="PlainTextNodeBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 纯文本文本内容。 </param>
    /// <param name="width"> 节点的宽度。 </param>
    public PlainTextNodeBuilder(string? text, NodeWidth? width = null)
    {
        Text = text;
        Width = width;
    }

    /// <inheritdoc />
    public NodeType Type => NodeType.PlainText;

    /// <inheritdoc />
    public string? Text { get; set; }

    /// <inheritdoc />
    public NodeWidth? Width { get; set; }

    /// <summary>
    ///     设置纯文本的文本内容，值将被设置到 <see cref="Text"/> 属性上。
    /// </summary>
    /// <param name="text"> 纯文本的文本内容。 </param>
    /// <returns> 当前构建器。 </returns>
    public PlainTextNodeBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    /// <summary>
    ///     设置节点的宽度，值将被设置到 <see cref="Width"/> 属性上。
    /// </summary>
    /// <param name="width"> 节点的宽度。 </param>
    /// <returns> 当前构建器。 </returns>
    public PlainTextNodeBuilder WithWidth(NodeWidth width)
    {
        Width = width;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="PlainTextNode"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="PlainTextNode"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Text"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Text"/> 的长度超过了 <see cref="MaxPlainTextLength"/>。
    /// </exception>
    [MemberNotNull(nameof(Text))]
    public PlainTextNode Build()
    {
        if (Text == null)
            throw new ArgumentNullException(nameof(Text), $"The {nameof(Text)} cannot be null.");

        if (Text.Length > MaxPlainTextLength)
            throw new ArgumentException(
                $"PlainText length must be less than or equal to {MaxPlainTextLength}.",
                nameof(Text));

        return new PlainTextNode(Text, Width);
    }

    /// <summary>
    ///     使用指定的纯文本文本内容初始化一个新的 <see cref="PlainTextNodeBuilder"/> 类的实例。
    /// </summary>
    /// <param name="text"> 纯文本文本内容。 </param>
    /// <returns> 一个使用指定的纯文本文本内容初始化的 <see cref="PlainTextNodeBuilder"/> 类的实例。 </returns>
    public static implicit operator PlainTextNodeBuilder(string text) => new(text);

    /// <inheritdoc />
    [MemberNotNull(nameof(Text))]
    INode INodeBuilder.Build() => Build();

    /// <inheritdoc />
    [MemberNotNull(nameof(Text))]
    ITextNode ITextNodeBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="PlainTextNodeBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="PlainTextNodeBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(PlainTextNodeBuilder? left, PlainTextNodeBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="PlainTextNodeBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="PlainTextNodeBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(PlainTextNodeBuilder? left, PlainTextNodeBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PlainTextNodeBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] PlainTextNodeBuilder? plainTextNodeBuilder)
    {
        if (plainTextNodeBuilder is null)
            return false;
        return Type == plainTextNodeBuilder.Type
            && Text == plainTextNodeBuilder.Text
            && Width == plainTextNodeBuilder.Width;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<INodeBuilder>.Equals([NotNullWhen(true)] INodeBuilder? nodeBuilder) =>
        Equals(nodeBuilder as PlainTextNodeBuilder);

    /// <inheritdoc />
    INode INodeBuilder.Build(IModuleBuilder parent) => Build();
}
