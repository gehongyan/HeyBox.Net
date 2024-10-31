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
    ///     获取此表情符号的扩展名。
    /// </summary>
    string Extension { get; }

    /// <summary>
    ///     获取此表情符号的创建时间。
    /// </summary>
    DateTimeOffset? CreatedAt { get; }

    /// <summary>
    ///     获取此表情符号的创建者。
    /// </summary>
    IRoomUser? Creator { get; }

    /// <summary>
    ///     获取此表情符号的创建者的 ID。
    /// </summary>
    ulong? CreatorId { get; }

    /// <summary>
    ///     获取此表情符号的路径。
    /// </summary>
    ulong Path { get; }
}
