namespace HeyBox.WebSocket;
using Model = API.Gateway.ChannelBaseInfo;

/// <summary>
///     表示一个基于网关的房间频道。
/// </summary>
public class SocketRoomChannel : SocketChannel, IRoomChannel
{
    /// <inheritdoc cref="HeyBox.IRoomChannel.Room" />
    public SocketRoom Room { get; }

    /// <inheritdoc />
    public string? Name { get; private set; }

    /// <inheritdoc />
    public ChannelType Type { get; internal set; }

    internal SocketRoomChannel(HeyBoxSocketClient client, ulong id, SocketRoom room)
        : base(client, id)
    {
        Room = room;
    }

    internal static SocketRoomChannel Create(SocketRoom room, ulong id, ChannelType type) =>
        type switch
        {
            ChannelType.Text => new SocketTextChannel(room.Client, id, room),
            _ => new SocketRoomChannel(room.Client, id, room)
        };

    internal static SocketRoomChannel Create(SocketRoom room, ClientState state, Model model) =>
        model.ChannelType switch
        {
            ChannelType.Text => SocketTextChannel.Create(room, state, model),
            _ => new SocketRoomChannel(room.Client, model.ChannelId, room)
        };

    internal override void Update(ClientState state, Model model)
    {
        Name = model.ChannelName;

        IsPopulated = true;
    }

    #region IRoomChannel

    /// <inheritdoc />
    IRoom IRoomChannel.Room => Room;

    /// <inheritdoc />
    ulong IRoomChannel.RoomId => Room.Id;

    #endregion
}
