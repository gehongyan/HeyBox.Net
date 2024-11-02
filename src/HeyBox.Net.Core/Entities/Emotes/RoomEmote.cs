using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     表示一个房间小表情符号。
/// </summary>hey
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RoomEmote : Emote, IRoomEmote, IEquatable<RoomEmote>
{
    /// <inheritdoc />
    public IRoom? Room { get; internal set; }

    /// <inheritdoc />
    public ulong RoomId { get; }

    /// <inheritdoc />
    public string Extension { get; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; }

    /// <inheritdoc />
    public IRoomUser? Creator { get; internal set; }

    /// <inheritdoc />
    public ulong? CreatorId { get; }

    /// <inheritdoc />
    public ulong Path { get; }

    /// <inheritdoc />
    public bool IsPopulated { get; }

    /// <summary>
    ///     初始化一个 <see cref="HeyBox.RoomEmote"/> 的新实例。
    /// </summary>
    /// <param name="roomId"> 表情符号所在的房间的 ID。 </param>
    /// <param name="path"> 表情符号的路径。 </param>
    /// <param name="extension"> 表情符号的扩展名。 </param>
    public RoomEmote(ulong roomId, ulong path, string extension)
        : base("custom", null)
    {
        RoomId = roomId;
        Path = path;
        Extension = extension;
    }

    internal RoomEmote(string name, ulong path, IRoom room, IRoomUser creator,
        string extension, DateTimeOffset? createdAt)
        : base("custom", name)
    {
        Extension = extension;
        Room = room;
        RoomId = room.Id;
        Path = path;
        CreatedAt = createdAt;
        Creator = creator;
        CreatorId = creator.Id;
        IsPopulated = true;
    }

    /// <summary>
    ///     尝试从一个表情符号的原始格式中解析出一个 <see cref="HeyBox.RoomEmote"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[custom3358126864697663488_1843946660894564352.png]</c>。
    /// </param>
    /// <param name="result"> 如果解析成功，则为解析出的 <see cref="HeyBox.RoomEmote"/>；否则为 <c>null</c>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    /// <example>
    ///     下面的示例演示了如何解析一个表情符号的原始格式：
    ///     <code language="cs">
    ///     bool success = Emote.TryParse("[custom3358126864697663488_1843946660894564352.png]", out Emote? emote)
    ///     </code>
    /// </example>
    public static bool TryParse(string text, [NotNullWhen(true)] out RoomEmote? result)
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
    ///     从一个表情符号的原始格式中解析出一个 <see cref="HeyBox.RoomEmote"/>。
    /// </summary>
    /// <param name="text">
    ///     表情符号的原始格式。例如 <c>[custom3358126864697663488_1843946660894564352.png]</c>。
    /// </param>
    /// <returns> 解析出的 <see cref="HeyBox.RoomEmote"/>。 </returns>
    /// <exception cref="ArgumentException">
    ///     无法解析 <paramref name="text"/> 为一个有效的表情符号。
    /// </exception>
    /// <example>
    ///     下面的示例演示了如何解析一个表情符号的原始格式：
    ///     <code language="cs">
    ///     RoomEmote emote = RoomEmote.Parse("[custom3358126864697663488_1843946660894564352.png]");
    ///     </code>
    /// </example>
    public static RoomEmote Parse(string text)
    {
        ReadOnlySpan<char> textSpan = text.AsSpan();
        if (!textSpan.StartsWith("[custom") || !textSpan.EndsWith("]"))
            throw new FormatException("The input text is not a valid emote format.");
        int underscoreIndex = textSpan.LastIndexOf('_');
        if (underscoreIndex == -1)
            throw new FormatException("The input text is not a valid emote format.");
        int dotIndex = textSpan.LastIndexOf('.');
        if (dotIndex == -1)
            throw new FormatException("The input text is not a valid emote format.");
        if (!ulong.TryParse(textSpan[7..underscoreIndex], out ulong roomId))
            throw new FormatException("The input text is not a valid emote format.");
        if (!ulong.TryParse(textSpan[(underscoreIndex + 1)..dotIndex], out ulong path))
            throw new FormatException("The input text is not a valid emote format.");
        string extension = textSpan[(dotIndex + 1)..^1].ToString();
        return new RoomEmote(roomId, path, extension);
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] RoomEmote? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return RoomId == other.RoomId && Path == other.Path && Extension == other.Extension;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is RoomEmote other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(RoomId, Path, Extension);

    private string DebuggerDisplay => $"{Name} ({Path}.{Extension})";

    /// <summary>
    ///     返回此表情符号的原始表示。
    /// </summary>
    /// <returns>
    ///     表示表情符号的原始表示（例如 <c>[custom3358126864697663488_1843946660894564352.png]</c>）。
    /// </returns>
    public override string ToString() => $"[custom{RoomId}_{Path}.{Extension}]";

    /// <inheritdoc />
    string IEmote.Group => "custom";
}
