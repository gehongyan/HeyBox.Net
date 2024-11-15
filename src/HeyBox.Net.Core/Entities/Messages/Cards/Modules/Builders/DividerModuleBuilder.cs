using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="DividerModule"/> 模块的构建器。
/// </summary>
public class DividerModuleBuilder : IModuleBuilder, IEquatable<DividerModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.Divider;

    /// <summary>
    ///     初始化一个 <see cref="DividerModuleBuilder"/> 类的新实例。
    /// </summary>
    public DividerModuleBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="DividerModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 分割线文本。 </param>
    public DividerModuleBuilder(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     设置分割线文本。
    /// </summary>
    /// <param name="text"> 要设置的分割线文本。 </param>
    /// <returns> 当前构建器。 </returns>
    public DividerModuleBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    /// <summary>
    ///     获取分割线模块的文本。
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="DividerModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="DividerModule"/> 对象。 </returns>
    public DividerModule Build() => new(Text);

    /// <inheritdoc />
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="DividerModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="DividerModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(DividerModuleBuilder? left, DividerModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="DividerModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="DividerModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(DividerModuleBuilder? left, DividerModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DividerModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] DividerModuleBuilder? dividerModuleBuilder)
    {
        if (dividerModuleBuilder is null)
            return false;

        return Type == dividerModuleBuilder.Type
            && Text == dividerModuleBuilder.Text;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as DividerModuleBuilder);
}
