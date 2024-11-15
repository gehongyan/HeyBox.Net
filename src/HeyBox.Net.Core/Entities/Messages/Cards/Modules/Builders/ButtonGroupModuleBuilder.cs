using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="ButtonGroupModule"/> 模块的构建器。
/// </summary>
public class ButtonGroupModuleBuilder : IModuleBuilder, IEquatable<ButtonGroupModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <summary>
    ///     按钮的最大数量。
    /// </summary>
    public const int MaxButtonCount = 3;

    /// <summary>
    ///     初始化一个 <see cref="ButtonGroupModuleBuilder"/> 类的新实例。
    /// </summary>
    public ButtonGroupModuleBuilder()
    {
        Nodes = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="ButtonGroupModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="nodes"> 按钮组模块要包含的按钮元素。 </param>
    public ButtonGroupModuleBuilder(IList<ButtonNodeBuilder> nodes)
    {
        Nodes = nodes;
    }

    /// <inheritdoc />
    public ModuleType Type => ModuleType.ButtonGroup;

    /// <summary>
    ///     获取或设置按钮组模块的按钮元素。
    /// </summary>
    public IList<ButtonNodeBuilder> Nodes { get; set; }

    /// <summary>
    ///     添加一个按钮元素到按钮组模块。
    /// </summary>
    /// <param name="field"> 要添加的按钮元素。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonGroupModuleBuilder AddNode(ButtonNodeBuilder field)
    {
        Nodes.Add(field);
        return this;
    }

    /// <summary>
    ///     添加一个按钮元素到按钮组模块。
    /// </summary>
    /// <param name="button"> 一个包含对要添加的新创建的按钮元素进行配置的操作的委托。 </param>
    /// <returns> 当前构建器。 </returns>
    public ButtonGroupModuleBuilder AddNode(Action<ButtonNodeBuilder>? button = null)
    {
        ButtonNodeBuilder field = new();
        button?.Invoke(field);
        Nodes.Add(field);
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="ButtonGroupModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="ButtonGroupModule"/> 对象。 </returns>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="Nodes"/> 为 <c>null</c>。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Nodes"/> 是一个空列表。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <see cref="Nodes"/> 的元素数量超过了 <see cref="MaxButtonCount"/>。
    /// </exception>
    public ButtonGroupModule Build()
    {
        if (Nodes == null)
            throw new ArgumentNullException(
                nameof(Nodes), "Node cannot be null or empty list.");
        if (Nodes.Count == 0)
            throw new ArgumentException(
                "Node cannot be null or empty list.", nameof(Nodes));
        if (Nodes.Count > MaxButtonCount)
            throw new ArgumentException(
                $"Node count must be less than or equal to {MaxButtonCount}.", nameof(Nodes));
        return new ButtonGroupModule([..Nodes.Select(e => e.Build())]);
    }

    /// <inheritdoc />
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="ButtonGroupModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonGroupModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ButtonGroupModuleBuilder? left, ButtonGroupModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ButtonGroupModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonGroupModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ButtonGroupModuleBuilder? left, ButtonGroupModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ButtonGroupModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ButtonGroupModuleBuilder? buttonGroupModuleBuilder)
    {
        if (buttonGroupModuleBuilder is null)
            return false;

        if (Nodes.Count != buttonGroupModuleBuilder.Nodes.Count)
            return false;

        if (Nodes
            .Zip(buttonGroupModuleBuilder.Nodes, (x, y) => (x, y))
            .Any(pair => pair.x != pair.y))
            return false;

        return Type == buttonGroupModuleBuilder.Type;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as ButtonGroupModuleBuilder);
}
