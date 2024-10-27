namespace HeyBox;

/// <summary>
///     表示一个通用的房间表情。
/// </summary>
public interface IRoomEmote : IEmote
{
    /// <summary>
    ///     获取此表情符号所在的房间。
    /// </summary>
    IRoom? Room { get; }

    /// <summary>
    ///     获取此表情符号所在的房间的 ID。
    /// </summary>
    ulong RoomId { get; }

    /// <summary>
    ///     获取此表情符号的创建者。
    /// </summary>
    IRoomUser? Creator { get; }

    /// <summary>
    ///     获取此表情符号的创建者的 ID。
    /// </summary>
    ulong? CreatorId { get; }
}
