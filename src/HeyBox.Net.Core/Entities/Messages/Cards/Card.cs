using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     表示一个卡片对象，可用于卡片消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Card : ICard, IEquatable<Card>, IEquatable<ICard>
{
    /// <inheritdoc />
    public CardType Type => CardType.Card;

    internal Card(ImmutableArray<IModule> modules)
    {
        Modules = modules;
    }

    /// <summary>
    ///     获取卡片的模块。
    /// </summary>
    public ImmutableArray<IModule> Modules { get; }

    private string DebuggerDisplay => $"{Type} ({Modules.Length} Modules)";

    /// <summary>
    ///     判定两个 <see cref="Card"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="Card"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(Card left, Card right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="Card"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="Card"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(Card left, Card right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Card card && Equals(card);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Card? card) =>
        GetHashCode() == card?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ Type.GetHashCode();
            foreach (IModule module in Modules)
                hash = (hash * 16777619) ^ module.GetHashCode();
            return hash;
        }
    }

    bool IEquatable<ICard>.Equals([NotNullWhen(true)] ICard? card) =>
        Equals(card as Card);

    /// <inheritdoc />
    IReadOnlyCollection<IModule> ICard.Modules => Modules;
}
