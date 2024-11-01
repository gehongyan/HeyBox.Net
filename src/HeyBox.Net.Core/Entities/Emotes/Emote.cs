using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     表示一个小表情符号。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class Emote : IEmote, IEquatable<Emote>
{
    /// <inheritdoc />
    public string Group { get; }

    /// <inheritdoc />
    public string? Name { get; }

    internal Emote(string group, string? name)
    {
        Group = group;
        Name = name;
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Emote? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Group == other.Group && Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Emote other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Group, Name);

    private string DebuggerDisplay => $"[{Group}_{Name}]";
}
