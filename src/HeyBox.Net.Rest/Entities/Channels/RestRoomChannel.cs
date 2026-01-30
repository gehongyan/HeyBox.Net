namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的房间频道。
/// </summary>
public class RestRoomChannel : RestChannel, IRoomChannel
{
    /// <inheritdoc cref="HeyBox.IRoomChannel.Room" />
    public IRoom Room { get; }

    /// <inheritdoc />
    public ulong RoomId => Room.Id;

    /// <inheritdoc />
    public ChannelType Type { get; internal set; }

    /// <inheritdoc />
    public ulong? CreatorId { get; internal set; }

    /// <inheritdoc />
    public IReadOnlyCollection<RolePermissionOverwrite> RolePermissionOverwrites => [];

    internal RestRoomChannel(BaseHeyBoxClient heyBox, IRoom room, ulong id)
        : base(heyBox, id)
    {
        Room = room;
        Type = ChannelType.Unspecified;
    }

    #region IRoomChannel

    /// <inheritdoc />
    string IChannel.Name => string.Empty;

    /// <inheritdoc />
    IRoom IRoomChannel.Room => Room;

    #endregion
}
