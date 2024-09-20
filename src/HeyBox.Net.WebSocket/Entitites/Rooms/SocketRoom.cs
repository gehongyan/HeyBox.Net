using System.Collections.Concurrent;
using HeyBox.API.Gateway;
using Model = HeyBox.API.Gateway.RoomBaseInfo;
using ChannelModel = HeyBox.API.Gateway.ChannelBaseInfo;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的房间。
/// </summary>
public class SocketRoom : SocketEntity<ulong>, IRoom
{
    private readonly ConcurrentDictionary<ulong, SocketRoomChannel> _channels;
    private readonly ConcurrentDictionary<ulong, SocketRoomUser> _members;
    private readonly ConcurrentDictionary<ulong, SocketRole> _roles;

    /// <inheritdoc />
    public string? Name { get; private set; }

    /// <inheritdoc />
    public string? Icon { get; private set; }

    /// <summary>
    ///     获取此房间内已缓存的成员数量。
    /// </summary>
    public int DownloadedMemberCount => _members.Count;

    /// <summary>
    ///     获取当前登录的用户。
    /// </summary>
    public SocketRoomUser? CurrentUser =>
        Client.CurrentUser is not null
        && _members.TryGetValue(Client.CurrentUser.Id, out SocketRoomUser? member)
            ? member
            : null;

    /// <summary>
    ///     获取此房间内缓存的所有用户。
    /// </summary>
    public IReadOnlyCollection<SocketRoomUser> Users => _members.ToReadOnlyCollection();

    /// <inheritdoc cref="HeyBox.IRoom.EveryoneRole" />
    public SocketRole EveryoneRole => GetRole(0) ?? new SocketRole(this, 0);

    internal SocketRoom(HeyBoxSocketClient client, ulong id)
        : base(client, id)
    {
        _channels = [];
        _members = [];
        _roles = [];
    }

    internal static SocketRoom Create(HeyBoxSocketClient client, ClientState state, Model model)
    {
        SocketRoom entity = new(client, model.RoomId);
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
    {
        Name = model.RoomName;
        Icon = model.RoomAvatar;
    }

    #region Channels

    /// <summary>
    ///     获取此房间内指定具有文字聊天能力的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    /// <remarks>
    ///     语音频道也是一种文字频道，此方法本意用于获取具有文字聊天能力的频道。如果通过此方法传入的 ID 对应的频道是语音频道，那么也会返回对应的语音频道实体。
    ///     如需获取频道的实际类型，请参考 <see cref="HeyBox.ChannelExtensions.GetChannelType(HeyBox.IChannel)"/>。
    /// </remarks>
    public SocketTextChannel GetTextChannel(ulong id) =>
        Client.State.GetChannel(id) as SocketTextChannel ?? new SocketTextChannel(Client, id, this);

    internal SocketRoomChannel AddOrUpdateChannel(ulong id)
    {
        if (_channels.TryGetValue(id, out SocketRoomChannel? cachedChannel))
            return cachedChannel;

        SocketRoomChannel channel = new(Client, id, this);
        _channels[id] = channel;
        Client.State.AddChannel(channel);
        return channel;
    }

    internal SocketRoomChannel AddOrUpdateChannel(ChannelModel model)
    {
        if (_channels.TryGetValue(model.ChannelId, out SocketRoomChannel? cachedChannel))
        {
            cachedChannel.Update(Client.State, model);
            return cachedChannel;
        }

        SocketRoomChannel channel = SocketRoomChannel.Create(this, Client.State, model);
        _channels[channel.Id] = channel;
        Client.State.AddChannel(channel);
        return channel;
    }

    internal SocketRoomChannel? RemoveChannel(ulong id) =>
        _channels.TryRemove(id, out SocketRoomChannel? _)
            ? Client.State.RemoveChannel(id) as SocketRoomChannel
            : null;

    internal void PurgeChannelCache()
    {
        foreach (KeyValuePair<ulong, SocketRoomChannel> channelId in _channels)
            Client.State.RemoveChannel(channelId.Key);
        _channels.Clear();
    }

    #endregion

    #region Roles

    /// <inheritdoc cref="HeyBox.IRoom.GetRole(System.UInt32)" />
    public SocketRole? GetRole(ulong id) => _roles.GetValueOrDefault(id);

    internal SocketRole AddOrUpdateRole(ulong roleId)
    {
        if (_roles.TryGetValue(roleId, out SocketRole? cachedRole))
            return cachedRole;

        SocketRole role = new(this, roleId);
        _roles[role.Id] = role;
        return role;
    }

    #endregion

    #region Users

    /// <summary>
    ///     获取此房间内的用户。
    /// </summary>
    /// <remarks>
    ///     此方法可能返回 <c>null</c>，因为在大型房间中，用户列表的缓存可能不完整。
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketRoomUser? GetUser(ulong id) => _members.GetValueOrDefault(id);

    internal SocketRoomUser AddOrUpdateUser(uint userId)
    {
        if (_members.TryGetValue(userId, out SocketRoomUser? cachedMember))
            return cachedMember;

        SocketRoomUser member = SocketRoomUser.Create(this, Client.State, userId);
        _members[member.Id] = member;
        return member;
    }

    internal SocketRoomUser AddOrUpdateUser(SenderInfo model)
    {
        if (_members.TryGetValue(model.UserId, out SocketRoomUser? cachedMember))
        {
            cachedMember.Update(Client.State, model);
            return cachedMember;
        }

        SocketRoomUser member = SocketRoomUser.Create(this, Client.State, model);
        member.GlobalUser.AddRef();
        _members[member.Id] = member;
        return member;
    }

    /// <summary>
    ///     清除此房间的用户缓存。
    /// </summary>
    public void PurgeUserCache() => PurgeUserCache(_ => true);

    /// <summary>
    ///     清除此房间的用户缓存。
    /// </summary>
    /// <param name="predicate"> 要清除的用户的筛选条件。 </param>
    public void PurgeUserCache(Predicate<SocketRoomUser> predicate)
    {
        IEnumerable<SocketRoomUser> membersToPurge = Users
            .Where(x => predicate.Invoke(x) && x.Id != Client.CurrentUser?.Id);
        IEnumerable<SocketRoomUser> membersToKeep = Users
            .Where(x => !predicate.Invoke(x) || x.Id == Client.CurrentUser?.Id);
        foreach (SocketRoomUser member in membersToPurge)
        {
            if (_members.TryRemove(member.Id, out _))
                member.GlobalUser.RemoveRef(Client);
        }
        foreach (SocketRoomUser member in membersToKeep)
            _members.TryAdd(member.Id, member);
    }


    #endregion

    #region IRoom

    /// <inheritdoc />
    Task<ITextChannel?> IRoom.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<ITextChannel?>(GetTextChannel(id));

    /// <inheritdoc />
    IRole IRoom.EveryoneRole => EveryoneRole;

    /// <inheritdoc />
    IRole? IRoom.GetRole(uint id) => GetRole(id);

    #endregion
}
