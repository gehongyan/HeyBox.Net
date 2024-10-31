﻿using System.Diagnostics;
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

    /// <summary>
    ///     从一个表情符号的原始格式中解析出一个 <see cref="BuiltinEmote"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[cube_摸摸头]</c>。
    /// </param>
    /// <param name="result"> 如果解析成功，则为解析出的 <see cref="HeyBox.BuiltinEmote"/>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool TryParse(string text, [NotNullWhen(true)] out IEmote? result)
    {
        try
        {
            result = Parse(text);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    ///     尝试从一个表情符号的原始格式中解析出一个 <see cref="BuiltinEmote"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[cube_摸摸头]</c>。
    /// </param>
    /// <returns> 解析出的 <see cref="HeyBox.BuiltinEmote"/>。 </returns>
    /// <exception cref="ArgumentException">
    ///     无法解析 <paramref name="text"/> 为一个有效的表情符号。
    /// </exception>
    public static IEmote Parse(string text)
    {
        ReadOnlySpan<char> textSpan = text.AsSpan();
        if (textSpan.StartsWith("[custom"))
            return RoomEmote.Parse(text);
        int underscoreIndex = textSpan.LastIndexOf('_');
        if (underscoreIndex == -1)
            throw new FormatException("The input text is not a valid emote format.");
        if (ulong.TryParse(textSpan.Slice(1, underscoreIndex - 1), out ulong _))
            return Emoji.Parse(text);
        return BuiltinEmote.Parse(text);
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
