using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个房间小表情符号。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RoomEmote : IEmote
{
    /// <inheritdoc />
    public ulong Id { get; }

    /// <summary>
    ///     获取此表情符号所在的房间。
    /// </summary>
    public IRoom Room { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public ulong Path => Id;

    /// <inheritdoc />
    public string Extension { get; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; }

    /// <inheritdoc />
    public IRoomUser Creator { get; }

    internal RoomEmote(IRoom room, IRoomUser creator,
        string name, ulong path, string extension, DateTimeOffset createdAt)
    {
        Room = room;
        Creator = creator;
        Name = name;
        Id = path;
        Extension = extension;
        CreatedAt = createdAt;
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
}
