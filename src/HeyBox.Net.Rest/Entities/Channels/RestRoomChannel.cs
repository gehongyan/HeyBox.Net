using System.Diagnostics;

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

    internal RestRoomChannel(BaseHeyBoxClient heyBox, IRoom room, ulong id)
        : base(heyBox, id)
    {
        Room = room;
        Type = ChannelType.Unspecified;
    }

    internal static RestRoomChannel Create(BaseHeyBoxClient client, IRoom room, ChannelType type, ulong id) =>
        type switch
        {
            ChannelType.Text => RestTextChannel.Create(client, room, id),
            _ => new RestRoomChannel(client, room, id)
        };

}
