using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="Card"/> 卡片的构建器。
/// </summary>
public class CardBuilder : ICardBuilder, IEquatable<CardBuilder>, IEquatable<ICardBuilder>
{
    /// <summary>
    ///     获取卡片的最大模块数量。
    /// </summary>
    public const int MaxModuleCount = 10;

    /// <inheritdoc />
    public CardType Type => CardType.Card;

    /// <summary>
    ///     初始化一个 <see cref="CardBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="color"> 卡片侧边的颜色。 </param>
    /// <param name="size"> 卡片的大小。 </param>
    /// <param name="modules"> 卡片的模块。 </param>
    public CardBuilder(CssColor? color = null, CardSize size = CardSize.Medium, IList<IModuleBuilder>? modules = null)
    {
        Color = color;
        Size = size;
        Modules = modules ?? [];
    }

    /// <summary>
    ///     获取或设置卡片左侧边的 CSS 颜色。
    /// </summary>
    /// <remarks>
    ///     未设置时等效于 <see cref="HeyBox.CssColor.Default"/>。
    /// </remarks>
    public CssColor? Color { get; set; }

    /// <summary>
    ///     获取或设置卡片的大小。
    /// </summary>
    public CardSize Size { get; set; }

    /// <summary>
    ///     获取或设置卡片的模块。
    /// </summary>
    public IList<IModuleBuilder> Modules { get; set; }

    /// <summary>
    ///     设置卡片侧边的颜色。
    /// </summary>
    /// <param name="color"> 卡片侧边的颜色。 </param>
    /// <returns> 当前构建器。 </returns>
    public CardBuilder WithColor(CssColor? color)
    {
        Color = color;
        return this;
    }

    /// <summary>
    ///     设置卡片的大小。
    /// </summary>
    /// <param name="size"> 卡片的大小。 </param>
    /// <returns> 当前构建器。 </returns>
    public CardBuilder WithSize(CardSize size)
    {
        Size = size;
        return this;
    }

    /// <summary>
    ///     添加一个模块到卡片。
    /// </summary>
    /// <param name="module"> 要添加的模块。 </param>
    /// <returns> 当前构建器。 </returns>
    public CardBuilder AddModule(IModuleBuilder module)
    {
        Modules.Add(module);
        return this;
    }

    /// <summary>
    ///     添加一个模块到卡片。
    /// </summary>
    /// <param name="action"> 一个包含对要添加的新创建的模块进行配置的操作的委托。 </param>
    /// <returns> 当前构建器。 </returns>
    public CardBuilder AddModule<T>(Action<T>? action = null)
        where T : IModuleBuilder, new()
    {
        T module = new();
        action?.Invoke(module);
        AddModule(module);
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="Card"/>。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="Card"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Modules"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Modules"/> 是一个空列表。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Modules"/> 的元素数量超过了 <see cref="MaxModuleCount"/>。
    /// </exception>
    public Card Build()
    {
        if (Modules is null)
            throw new ArgumentNullException(nameof(Modules), "The modules of the card cannot be null.");
        if (Modules.Count == 0)
            throw new ArgumentException("The modules of the card cannot be empty.", nameof(Modules));
        if (Modules.Count > MaxModuleCount)
            throw new ArgumentException($"The modules of the card cannot exceed {MaxModuleCount}.", nameof(Modules));
        return new Card(Color, Size, [..Modules.Select(m => m.Build())]);
    }

    /// <inheritdoc />
    ICard ICardBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="CardBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="CardBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(CardBuilder? left, CardBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="CardBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="CardBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(CardBuilder? left, CardBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CardBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] CardBuilder? cardBuilder)
    {
        if (cardBuilder is null)
            return false;

        if (Modules.Count != cardBuilder.Modules.Count)
            return false;

        if (Modules
            .Zip(cardBuilder.Modules, (x, y) => (x, y))
            .Any(pair => pair.x != pair.y))
            return false;

        return Type == cardBuilder.Type;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<ICardBuilder>.Equals([NotNullWhen(true)] ICardBuilder? cardBuilder) =>
        Equals(cardBuilder as CardBuilder);
}
