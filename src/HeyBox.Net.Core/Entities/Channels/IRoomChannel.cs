namespace HeyBox;

/// <summary>
///     表示一个通用的房间频道。
/// </summary>
public interface IRoomChannel : IChannel
{
    /// <summary>
    ///     获取此频道所属的房间。
    /// </summary>
    IRoom Room { get; }

    /// <summary>
    ///     获取此频道所属的房间的 ID。
    /// </summary>
    ulong RoomId { get; }

    /// <summary>
    ///     获取此频道的类型。
    /// </summary>
    ChannelType Type { get; }
}
