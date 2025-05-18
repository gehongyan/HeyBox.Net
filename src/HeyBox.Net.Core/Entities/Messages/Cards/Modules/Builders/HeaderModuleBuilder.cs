using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="HeaderModule"/> 模块的构建器。
/// </summary>
public class HeaderModuleBuilder : IModuleBuilder, IEquatable<HeaderModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <summary>
    ///     标题内容内容的最大长度。
    /// </summary>
    public const int MaxHeaderContentLength = 100;

    /// <inheritdoc />
    public ModuleType Type => ModuleType.Header;

    /// <summary>
    ///     初始化一个 <see cref="HeaderModuleBuilder"/> 类的新实例。
    /// </summary>
    public HeaderModuleBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="HeaderModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="content"> 标题内容。 </param>
    public HeaderModuleBuilder(ITextNodeBuilder content)
    {
        Content = content;
    }

    /// <summary>
    ///     初始化一个 <see cref="HeaderModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 标题内容的文本。 </param>
    /// <param name="isMarkdown"> 是否为 Markdown 格式。 </param>
    public HeaderModuleBuilder(string text, bool isMarkdown = false)
    {
        Content = isMarkdown ? new MarkdownNodeBuilder(text) : new PlainTextNodeBuilder(text);
    }

    /// <summary>
    ///     获取或设置标题内容。
    /// </summary>
    public ITextNodeBuilder? Content { get; set; }

    /// <summary>
    ///     设置标题内容。
    /// </summary>
    /// <param name="content"> 要设置的标题内容。 </param>
    /// <returns> 当前构建器。 </returns>
    public HeaderModuleBuilder WithContent(ITextNodeBuilder content)
    {
        Content = content;
        return this;
    }

    /// <summary>
    ///     设置标题内容。
    /// </summary>
    /// <param name="content"> 要设置的标题内容。 </param>
    /// <param name="isMarkdown"> 是否为 Markdown 格式。 </param>
    /// <returns> 当前构建器。 </returns>
    public HeaderModuleBuilder WithContent(string content, bool isMarkdown = false)
    {
        Content = isMarkdown ? new MarkdownNodeBuilder(content) : new PlainTextNodeBuilder(content);
        return this;
    }

    /// <summary>
    ///     设置标题内容。
    /// </summary>
    /// <param name="action"> 一个包含对要设置的标题内容进行配置的操作的委托。 </param>
    /// <returns> 当前构建器。 </returns>
    public HeaderModuleBuilder WithContent<T>(Action<T>? action = null)
        where T : ITextNodeBuilder, new()
    {
        T text = new();
        action?.Invoke(text);
        Content = text;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="HeaderModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="HeaderModule"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Content"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Content"/> 的内容为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Content"/> 的内容长度超过了 <see cref="MaxHeaderContentLength"/>。
    /// </exception>
    [MemberNotNull(nameof(Content))]
    public HeaderModule Build()
    {
        if (Content is null)
            throw new ArgumentNullException(nameof(Content), "The header text cannot be null.");

        if (Content.Text is null)
            throw new ArgumentException("The content of the header text cannot be null.", nameof(Content));

        if (Content.Text.Length > MaxHeaderContentLength)
            throw new ArgumentException(
                $"Header content length must be less than or equal to {MaxHeaderContentLength}.",
                nameof(Content));

        return new HeaderModule(Content.Build());
    }

    /// <summary>
    ///     使用指定的纯内容内容内容初始化一个新的 <see cref="HeaderModuleBuilder"/> 类的实例。
    /// </summary>
    /// <param name="text"> 纯内容内容内容。 </param>
    /// <returns> 一个使用指定的纯内容内容内容初始化的 <see cref="PlainTextNodeBuilder"/> 类的实例。 </returns>
    public static implicit operator HeaderModuleBuilder(string text) => new(text);

    /// <inheritdoc />
    [MemberNotNull(nameof(Content))]
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="HeaderModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="HeaderModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(HeaderModuleBuilder? left, HeaderModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="HeaderModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="HeaderModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(HeaderModuleBuilder? left, HeaderModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is HeaderModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] HeaderModuleBuilder? headerModuleBuilder)
    {
        if (headerModuleBuilder == null)
            return false;

        return Type == headerModuleBuilder.Type
            && Content == headerModuleBuilder.Content;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as HeaderModuleBuilder);
}
