using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     表示一个通用的表情符号。
/// </summary>
public interface IEmote
{
    /// <summary>
    ///     获取此表情符号的分组。
    /// </summary>
    string Group { get; }

    /// <summary>
    ///     获取此表情符号的显示名称。
    /// </summary>
    string? Name { get; }

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
}
