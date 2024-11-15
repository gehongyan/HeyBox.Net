using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     按钮组模块，可用于 <see cref="ICard"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ButtonGroupModule : IModule, IEquatable<ButtonGroupModule>, IEquatable<IModule>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.ButtonGroup;

    internal ButtonGroupModule(ImmutableArray<ButtonNode> buttons)
    {
        Buttons = buttons;
    }

    /// <summary>
    ///     获取按钮组模块的按钮。
    /// </summary>
    public ImmutableArray<ButtonNode> Buttons { get; }

    private string DebuggerDisplay => $"{Type} ({Buttons.Length} Button{(Buttons.Length == 1 ? string.Empty : "s")})";

    /// <summary>
    ///     判定两个 <see cref="ButtonGroupModule"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonGroupModule"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ButtonGroupModule left, ButtonGroupModule right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ButtonGroupModule"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonGroupModule"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ButtonGroupModule left, ButtonGroupModule right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ButtonGroupModule buttonGroupModule && Equals(buttonGroupModule);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ButtonGroupModule? buttonGroupModule) =>
        GetHashCode() == buttonGroupModule?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ Type.GetHashCode();
            foreach (ButtonNode buttonNode in Buttons)
                hash = (hash * 16777619) ^ buttonNode.GetHashCode();
            return hash;
        }
    }

    bool IEquatable<IModule>.Equals([NotNullWhen(true)] IModule? module) =>
        Equals(module as ButtonGroupModule);
}
