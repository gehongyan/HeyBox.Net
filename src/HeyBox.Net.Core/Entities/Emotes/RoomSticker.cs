using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个房间大表情符号。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RoomSticker : Emote, IRoomEmote
{
    /// <inheritdoc />
    public IRoom? Room { get; }

    /// <inheritdoc />
    public ulong RoomId { get; }

    /// <inheritdoc />
    public string? Extension { get; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; }

    /// <inheritdoc />
    public IRoomUser? Creator { get; }

    /// <inheritdoc />
    public ulong? CreatorId { get; }

    /// <inheritdoc />
    public ulong? Path { get; }

    internal RoomSticker(ulong roomId, ulong path, string extension)
        : base("custom", null)
    {
        RoomId = roomId;
        Path = path;
        Extension = extension;
    }

    internal RoomSticker(string name, ulong path, IRoom room, IRoomUser creator,
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
    }

    private string DebuggerDisplay => $"{Name} ({Path}.{Extension})";

    /// <summary>
    ///     返回此表情符号的原始表示。
    /// </summary>
    /// <returns>
    ///     表示表情符号的原始表示（例如 <c>[custom3358126864697663488_demo]</c>）。
    /// </returns>
    public override string ToString() => $"[custom{RoomId}_{Name}]";

    /// <inheritdoc />
    string IEmote.Group => "custom";
}
