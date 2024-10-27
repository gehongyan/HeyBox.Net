using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个房间大表情符号。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RoomSticker : Emote, IRoomEmote
{
    /// <summary>
    ///     获取此表情符号所在的房间。
    /// </summary>
    public IRoom Room { get; }

    /// <summary>
    ///     获取此表情符号所在的房间的 ID。
    /// </summary>
    public ulong RoomId => Room.Id;

    /// <summary>
    ///     获取此表情符号的创建者。
    /// </summary>
    public IRoomUser Creator { get; }

    /// <summary>
    ///     获取此表情符号的创建者的 ID。
    /// </summary>
    public ulong CreatorId => Creator.Id;

    internal RoomSticker(IRoom room, IRoomUser creator,
        string name, ulong path, string extension, DateTimeOffset createdAt)
        : base(name, path, extension, createdAt)
    {
        Room = room;
        Creator = creator;
    }

    private string DebuggerDisplay => $"{Name} ({Path}.{Extension})";

    /// <summary>
    ///     返回此表情符号的原始表示。
    /// </summary>
    /// <returns>
    ///     表示表情符号的原始表示（例如 <c>[custom3358126864697663488_1843946660894564352.png]</c>）。
    /// </returns>
    public override string ToString() => $"[custom{Room.Id}_{Path}.{Extension}]";

    /// <inheritdoc />
    bool IEntity<ulong>.IsPopulated => true;

    /// <inheritdoc />
    ulong? IRoomEmote.CreatorId => CreatorId;
}
