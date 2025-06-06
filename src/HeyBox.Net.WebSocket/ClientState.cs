﻿using System.Collections.Concurrent;

namespace HeyBox.WebSocket;

internal class ClientState
{
    // TODO: To be investigated for HeyBox
    private const double AverageChannelsPerRoom = 10.22; //Source: Googie2149
    private const double AverageUsersPerRoom = 47.78;    //Source: Googie2149
    private const double CollectionMultiplier = 1.05;    //Add 5% buffer to handle growth

    private readonly ConcurrentDictionary<ulong, SocketChannel> _channels;
    private readonly ConcurrentDictionary<uint, SocketDMChannel> _dmChannels;
    private readonly ConcurrentDictionary<ulong, SocketRoom> _rooms;
    private readonly ConcurrentDictionary<uint, SocketGlobalUser> _users;

    internal IReadOnlyCollection<SocketChannel> Channels => _channels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketDMChannel> DMChannels => _dmChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketRoom> Rooms => _rooms.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGlobalUser> Users => _users.ToReadOnlyCollection();

    public ClientState(int roomCount, int dmChannelCount)
    {
        double estimatedChannelCount = roomCount * AverageChannelsPerRoom;
        double estimatedUsersCount = roomCount * AverageUsersPerRoom;
        _channels = new ConcurrentDictionary<ulong, SocketChannel>(ConcurrentHashSet.DefaultConcurrencyLevel,
            (int)(estimatedChannelCount * CollectionMultiplier));
        _dmChannels = new ConcurrentDictionary<uint, SocketDMChannel>(ConcurrentHashSet.DefaultConcurrencyLevel,
            (int)(dmChannelCount * CollectionMultiplier));
        _rooms = new ConcurrentDictionary<ulong, SocketRoom>(ConcurrentHashSet.DefaultConcurrencyLevel,
            (int)(roomCount * CollectionMultiplier));
        _users = new ConcurrentDictionary<uint, SocketGlobalUser>(ConcurrentHashSet.DefaultConcurrencyLevel,
            (int)(estimatedUsersCount * CollectionMultiplier));
    }

    #region Channels

    internal SocketChannel? GetChannel(ulong channelId) =>
        _channels.GetValueOrDefault(channelId);

    internal SocketDMChannel? GetDMChannel(uint userId) =>
        _dmChannels.Values.FirstOrDefault(x => x.Recipient.Id == userId);

    internal void AddChannel(SocketChannel channel) => _channels[channel.Id] = channel;

    internal void AddDMChannel(SocketDMChannel channel) => _dmChannels[channel.Id] = channel;

    internal SocketChannel? RemoveChannel(ulong id) =>
        _channels.TryRemove(id, out SocketChannel? channel) ? channel : null;

    #endregion

    #region Rooms

    internal SocketRoom? GetRoom(ulong roomId) =>
        _rooms.GetValueOrDefault(roomId);

    internal void AddRoom(SocketRoom room) => _rooms[room.Id] = room;

    internal SocketRoom GetOrAddRoom(ulong id, Func<ulong, SocketRoom> roomFactory) =>
        _rooms.GetOrAdd(id, roomFactory);

    internal SocketRoom? RemoveRoom(ulong id)
    {
        if (!_rooms.TryRemove(id, out SocketRoom? room))
            return null;
        room.PurgeChannelCache();
        room.PurgeUserCache();
        return room;
    }

    #endregion

    #region Users

    internal SocketGlobalUser? GetUser(uint id) =>
        _users.GetValueOrDefault(id);

    internal SocketGlobalUser GetOrAddUser(uint id, Func<uint, SocketGlobalUser> userFactory) =>
        _users.GetOrAdd(id, userFactory);

    internal SocketGlobalUser? RemoveUser(uint id) =>
        _users.TryRemove(id, out SocketGlobalUser? user) ? user : null;

    #endregion
}
