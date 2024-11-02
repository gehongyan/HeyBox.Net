using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     表示一个内置的 Emoji 表情符号。
/// </summary>
public class Emoji : Emote, IEquatable<Emoji>, IEntity<string>
{
    /// <summary>
    ///     初始化一个 <see cref="Emoji"/> 的新实例。
    /// </summary>
    /// <param name="group"> 表情符号的分组。 </param>
    /// <param name="name"> 表情符号的名称。 </param>
    public Emoji(string group, string name)
        : base(group, name)
    {
    }

    /// <summary>
    ///     从一个表情符号的原始格式中解析出一个 <see cref="Emoji"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[cube_摸摸头]</c>。
    /// </param>
    /// <param name="result"> 如果解析成功，则为解析出的 <see cref="Emoji"/>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool TryParse(string text, [NotNullWhen(true)] out Emoji? result)
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
    ///     尝试从一个表情符号的原始格式中解析出一个 <see cref="Emoji"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[cube_摸摸头]</c>。
    /// </param>
    /// <returns> 解析出的 <see cref="Emoji"/>。 </returns>
    /// <exception cref="ArgumentException">
    ///     无法解析 <paramref name="text"/> 为一个有效的表情符号。
    /// </exception>
    public static Emoji Parse(string text)
    {
        ReadOnlySpan<char> textSpan = text.AsSpan();
        if (textSpan is not ['[', .., ']'])
            throw new ArgumentException("Invalid emote format.", nameof(text));
        int underscoreIndex = textSpan.LastIndexOf('_');
        if (underscoreIndex == -1)
            throw new ArgumentException("Invalid emote format.", nameof(text));
        string group = textSpan[1..underscoreIndex].ToString();
        string name = textSpan[(underscoreIndex + 1)..^1].ToString();
        return new Emoji(group, name);
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Emoji? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Group == other.Group && Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Emoji other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Group, Name);

    /// <inheritdoc />
    public override string ToString() => $"[{Group}_{Name}]";

    /// <inheritdoc />
    bool IEntity<string>.IsPopulated => true;
}
