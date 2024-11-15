using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="ButtonNode"/> 元素的构建器。
/// </summary>
public class ButtonNodeBuilder : INodeBuilder, IEquatable<ButtonNodeBuilder>, IEquatable<INodeBuilder>
{
    /// <summary>
    ///     按钮文本的最大长度。
    /// </summary>
    public const int MaxButtonTextLength = 12;

    /// <summary>
    ///     初始化一个 <see cref="ButtonNodeBuilder"/> 类的新实例。
    /// </summary>
    public ButtonNodeBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="ButtonNodeBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 按钮的文本。 </param>
    /// <param name="event"> 按钮的点击事件类型。 </param>
    /// <param name="value"> 按钮的值。 </param>
    /// <param name="theme"> 按钮的主题。 </param>
    /// <remarks>
    ///     如果 <paramref name="event"/> 设置为 <see cref="HeyBox.ButtonEvent.LinkTo"/>，
    ///     则在用户点击按钮时，黑盒语音会将用户重定向到 <paramref name="value" /> 指定的 URL。<br />
    ///     无论 <paramref name="event"/> 设置为何值，用户点击按钮时，黑盒语音都会通过网关下发按钮点击事件，并携带所有按钮的属性值。
    /// </remarks>
    public ButtonNodeBuilder(string text, ButtonEvent @event, string value, ButtonTheme theme = ButtonTheme.Default)
    {
        Text = text;
        Event = @event;
        Value = value;
        Theme = theme;
    }

    /// <inheritdoc />
    public NodeType Type => NodeType.Button;

    /// <summary>
    ///     获取或设置按钮的主题。
    /// </summary>
    public ButtonTheme Theme { get; set; }

    /// <summary>
    ///     获取或设置按钮的值。
    /// </summary>
    /// <remarks>
    ///     如果 <see name="HeyBox.ButtonNodeBuilder.Event"/> 设置为 <see cref="HeyBox.ButtonEvent.LinkTo"/>，
    ///     则在用户点击按钮时，黑盒语音会将用户重定向到此属性值指定的 URL。<br />
    ///     无论设置为何值，用户点击按钮时，黑盒语音都会通过网关下发按钮点击事件，并携带所有按钮的属性值。
    /// </remarks>
    public string? Value { get; set; }

    /// <summary>
    ///     获取或设置按钮被点击时触发的事件类型。
    /// </summary>
    /// <remarks>
    ///     如果此属性的值设置为 <see cref="HeyBox.ButtonEvent.LinkTo"/>，
    ///     则在用户点击按钮时，黑盒语音会将用户重定向到 <paramref name="value" /> 指定的 URL。<br />
    ///     无论此设置为何值，用户点击按钮时，黑盒语音都会通过网关下发按钮点击事件，并携带所有按钮的属性值。
    /// </remarks>
    public ButtonEvent Event { get; set; }

    /// <summary>
    ///     获取或设置按钮的文本元素。
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     设置按钮的主题，值将被设置到 <see cref="Theme"/> 属性上。
    /// </summary>
    /// <param name="theme"> 按钮的主题。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonNodeBuilder WithTheme(ButtonTheme theme)
    {
        Theme = theme;
        return this;
    }

    /// <summary>
    ///     设置按钮的值，值将被设置到 <see cref="Value"/> 属性上。
    /// </summary>
    /// <param name="value"> 按钮的值。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonNodeBuilder WithValue(string value)
    {
        Value = value;
        return this;
    }

    /// <summary>
    ///     设置按钮被点击时触发的事件类型，值将被设置到 <see cref="Event"/> 属性上。
    /// </summary>
    /// <param name="event"> 按钮的点击事件类型。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonNodeBuilder WithEvent(ButtonEvent @event)
    {
        Event = @event;
        return this;
    }

    /// <summary>
    ///     设置按钮的文本。
    /// </summary>
    /// <param name="text"> 按钮的文本。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonNodeBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="ButtonNode"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="ButtonNode"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Text"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Text"/> 为空字符串。
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Value"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Value"/> 为空字符串。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     文本的长度超过了 <see cref="MaxButtonTextLength"/>。
    /// </exception>
    /// <exception cref="UriFormatException">
    ///     <see cref="Value"/> 不是有效的 URL。
    /// </exception>
    [MemberNotNull(nameof(Text))]
    public ButtonNode Build()
    {
        if (Text is null)
            throw new ArgumentNullException(nameof(Text), "The text of a button cannot be null.");

        if (string.IsNullOrEmpty(Text))
            throw new ArgumentException("The text of a button cannot be empty.", nameof(Text));

        if (Text.Length > MaxButtonTextLength)
        {
            throw new ArgumentException(
                $"The length of button text must be less than or equal to {MaxButtonTextLength}.",
                nameof(Text));
        }

        if (Value is null)
            throw new ArgumentNullException(nameof(Value), "The value of a button cannot be null.");

        if (string.IsNullOrEmpty(Value))
            throw new ArgumentException("The value of a button cannot be empty.", nameof(Value));

        if (Event == ButtonEvent.LinkTo)
            UrlValidation.Validate(Value);

        return new ButtonNode(Text, Event, Value, Theme);
    }

    /// <inheritdoc />
    [MemberNotNull(nameof(Text))]
    INode INodeBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="ButtonNodeBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonNodeBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ButtonNodeBuilder? left, ButtonNodeBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ButtonNodeBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonNodeBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ButtonNodeBuilder? left, ButtonNodeBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ButtonNodeBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ButtonNodeBuilder? buttonNodeBuilder)
    {
        if (buttonNodeBuilder is null)
            return false;

        return Type == buttonNodeBuilder.Type
            && Theme == buttonNodeBuilder.Theme
            && Value == buttonNodeBuilder.Value
            && Event == buttonNodeBuilder.Event
            && Text == buttonNodeBuilder.Text;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<INodeBuilder>.Equals([NotNullWhen(true)] INodeBuilder? nodeBuilder) =>
        Equals(nodeBuilder as ButtonNodeBuilder);

    /// <inheritdoc />
    INode INodeBuilder.Build(IModuleBuilder parent) => Build();
}
