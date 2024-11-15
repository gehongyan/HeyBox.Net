using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     分割线模块，可用于 <see cref="ICard"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class DividerModule : IModule, IEquatable<DividerModule>, IEquatable<IModule>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.Divider;

    internal DividerModule(string? text)
    {
        Text = text;
    }

    /// <summary>
    ///     获取分割线模块的文本。
    /// </summary>
    public string? Text { get; }

    private string DebuggerDisplay => $"{Type}{(Text is null ? string.Empty : $": {Text}")}";

    /// <summary>
    ///     判定两个 <see cref="DividerModule"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="DividerModule"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(DividerModule left, DividerModule right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="DividerModule"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="DividerModule"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(DividerModule left, DividerModule right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DividerModule dividerModule && Equals(dividerModule);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] DividerModule? dividerModule) =>
        GetHashCode() == dividerModule?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Type, Text);

    bool IEquatable<IModule>.Equals([NotNullWhen(true)] IModule? module) =>
        Equals(module as DividerModule);
}
