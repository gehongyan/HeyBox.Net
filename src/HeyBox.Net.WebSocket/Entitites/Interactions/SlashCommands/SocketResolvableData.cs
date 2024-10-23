namespace HeyBox.WebSocket;

internal class SocketResolvableData
{
    private readonly Func<ulong, SocketRoomChannel> _channelResolver;
    private readonly Func<ulong, SocketRole> _roleResolver;
    private readonly Func<uint, SocketUser> _userResolver;

    internal SocketResolvableData(SocketRoom room)
    {
        _channelResolver = room.AddOrUpdateChannel;
        _roleResolver = x => room.GetRole(x) ?? new SocketRole(room, x);
        _userResolver = room.AddOrUpdateUser;
    }

    public SocketRoomChannel ResolveChannel(ulong id) => _channelResolver(id);
    public SocketRole ResolveRole(ulong id) => _roleResolver(id);
    public SocketUser ResolveUser(uint id) => _userResolver(id);
}
