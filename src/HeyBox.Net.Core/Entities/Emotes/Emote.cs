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
    public ulong Id { get; }

    /// <inheritdoc />
    public string? Name { get; }

    /// <inheritdoc />
    public ulong Path => Id;

    /// <inheritdoc />
    public string Extension { get; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; }

    internal Emote(string? name, ulong path, string extension, DateTimeOffset? createdAt)
    {
        Name = name;
        Id = path;
        Extension = extension;
        CreatedAt = createdAt;
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Emote? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Emote other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();

    private string DebuggerDisplay => $"{Name} ({Path}.{Extension})";

    /// <inheritdoc />
    bool IEntity<ulong>.IsPopulated => true;
}
