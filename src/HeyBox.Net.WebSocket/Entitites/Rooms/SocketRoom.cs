using System.Collections.Concurrent;
using System.Diagnostics;
using HeyBox.API.Rest;
using HeyBox.Rest;
using Model = HeyBox.API.Gateway.RoomBaseInfo;
using ChannelModel = HeyBox.API.Gateway.ChannelBaseInfo;
using RoleModel = HeyBox.API.Role;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的房间。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketRoom : SocketEntity<ulong>, IRoom, IUpdateable
{
    private readonly ConcurrentDictionary<ulong, SocketRoomChannel> _channels;
    private readonly ConcurrentDictionary<ulong, SocketRoomUser> _members;
    private readonly ConcurrentDictionary<ulong, SocketRole> _roles;
    private readonly ConcurrentDictionary<ulong, RoomEmote> _emotes;
    private readonly ConcurrentDictionary<ulong, RoomSticker> _stickers;

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
    public SocketRole EveryoneRole => _roles.Values.SingleOrDefault(x => x.Type is RoleType.Everyone)
        ?? new SocketRole(this, 0) { Type = RoleType.Everyone };

    /// <inheritdoc cref="HeyBox.IRoom.Roles" />
    public IReadOnlyCollection<SocketRole> Roles => _roles.ToReadOnlyCollection();

    internal SocketRoom(HeyBoxSocketClient client, ulong id)
        : base(client, id)
    {
        _channels = [];
        _members = [];
        _roles = [];
        _emotes = [];
        _stickers = [];
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

    internal void Update(ClientState state, API.Rest.GetRoomRolesResponse model)
    {
        _roles.Clear();
        foreach (RoleModel roleModel in model.Roles)
        {
            SocketRole role = SocketRole.Create(this, state, roleModel);
            _roles.TryAdd(role.Id, role);
        }
    }

    internal void Update(ClientState state, GetRoomMemesResponse model)
    {
        _emotes.Clear();
        foreach (RoomMeme emojiInfo in model.Emojis)
        {
            SocketRoomUser creator = AddOrUpdateUser(emojiInfo.UserInfo);
            RoomEmote emote = emojiInfo.MemeInfo.ToEmoteEntity(creator);
            _emotes.TryAdd(emote.Path, emote);
        }

        _stickers.Clear();
        foreach (RoomMeme stickerInfo in model.Stickers)
        {
            SocketRoomUser creator = AddOrUpdateUser(stickerInfo.UserInfo);
            RoomSticker sticker = stickerInfo.MemeInfo.ToStickerEntity(creator);
            _stickers.TryAdd(sticker.Path, sticker);
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(RequestOptions? options = null)
    {
        await SocketRoomHelper.UpdateAsync(this, Client, options);

        IsPopulated = true;
    }

    /// <inheritdoc cref="HeyBox.WebSocket.SocketRoom.Name" />
    public override string? ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id})";

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

    internal SocketRoomChannel AddOrUpdateChannel(ulong id, ChannelType type = ChannelType.Unspecified)
    {
        if (_channels.TryGetValue(id, out SocketRoomChannel? cachedChannel))
            return cachedChannel;

        SocketRoomChannel channel = SocketRoomChannel.Create(this, id, type);
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

    /// <inheritdoc cref="HeyBox.IRoom.GetRole(System.UInt64)" />
    public SocketRole? GetRole(ulong id) => _roles.GetValueOrDefault(id);

    /// <inheritdoc cref="HeyBox.IRoom.CreateRoleAsync(System.Action{RoleProperties},HeyBox.RequestOptions)" />
    public async Task<SocketRole> CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options = null)
    {
        RoleModel model = await RoomHelper.CreateRoleAsync(this, Client, func, options);
        return AddOrUpdateRole(model.Id, model);
    }

    internal SocketRole AddOrUpdateRole(ulong roleId, RoleModel model)
    {
        if (_roles.TryGetValue(roleId, out SocketRole? cachedRole))
        {
            cachedRole.Update(Client.State, model);
            return cachedRole;
        }

        SocketRole role = SocketRole.Create(this, Client.State, model);
        _roles[role.Id] = role;
        return role;
    }

    internal SocketRole? RemoveRole(ulong id) => _roles.TryRemove(id, out SocketRole? role) ? role : null;

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

    internal SocketRoomUser AddOrUpdateUser(API.RoomUser model)
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

    internal SocketRoomUser? RemoveUser(ulong id)
    {
        if (!_members.TryRemove(id, out SocketRoomUser? member))
            return null;
        member.GlobalUser.RemoveRef(Client);
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

    #region Emotes

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<RoomEmote>> GetEmotesAsync(RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        Update(Client.State, model);
        return _emotes.ToReadOnlyCollection();
    }

    /// <inheritdoc />
    public async Task<RoomEmote?> GetEmoteAsync(ulong id, RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        Update(Client.State, model);
        return _emotes.GetValueOrDefault(id);
    }

    /// <inheritdoc />
    public Task ModifyEmoteAsync(RoomEmote emote, Action<EmoteProperties> func, RequestOptions? options = null) =>
        RoomHelper.ModifyMemeAsync(this, Client, emote, func, options);

    /// <inheritdoc />
    public Task DeleteEmoteAsync(RoomEmote emote, RequestOptions? options = null) =>
        RoomHelper.DeleteMemeAsync(this, Client, emote, options);

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<RoomSticker>> GetStickersAsync(RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        Update(Client.State, model);
        return _stickers.ToReadOnlyCollection();
    }

    /// <inheritdoc />
    public async Task<RoomSticker?> GetStickerAsync(ulong id, RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        Update(Client.State, model);
        return _stickers.GetValueOrDefault(id);
    }

    /// <inheritdoc />
    public Task ModifyStickerAsync(RoomSticker sticker, Action<EmoteProperties> func, RequestOptions? options = null) =>
        RoomHelper.ModifyMemeAsync(this, Client, sticker, func, options);

    /// <inheritdoc />
    public Task DeleteStickerAsync(RoomSticker sticker, RequestOptions? options = null) =>
        RoomHelper.DeleteMemeAsync(this, Client, sticker, options);

    #endregion

    #region IRoom

    /// <inheritdoc />
    Task<ITextChannel?> IRoom.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<ITextChannel?>(GetTextChannel(id));

    /// <inheritdoc />
    IReadOnlyCollection<IRole> IRoom.Roles => Roles;

    /// <inheritdoc />
    IRole IRoom.EveryoneRole => EveryoneRole;

    /// <inheritdoc />
    IRole? IRoom.GetRole(ulong id) => GetRole(id);

    /// <inheritdoc />
    async Task<IRole> IRoom.CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options) =>
        await CreateRoleAsync(func, options).ConfigureAwait(false);

    #endregion
}
