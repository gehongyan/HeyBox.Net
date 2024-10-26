using System.Collections.Immutable;
using HeyBox.API;
using HeyBox.API.Rest;
using RoleModel = HeyBox.API.Role;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的房间。
/// </summary>
public class RestRoom : RestEntity<ulong>, IRoom, IUpdateable
{
    private ImmutableDictionary<ulong, RestRole> _roles;

    internal RestRoom(BaseHeyBoxClient client, ulong id)
        : base(client, id)
    {
        _roles = ImmutableDictionary<ulong, RestRole>.Empty;
    }

    /// <inheritdoc cref="HeyBox.IRoom.Roles" />
    public IReadOnlyCollection<RestRole> Roles => _roles.ToReadOnlyCollection();

    /// <inheritdoc cref="HeyBox.IRoom.EveryoneRole" />
    public RestRole EveryoneRole => _roles.Values.SingleOrDefault(x => x.Type is RoleType.Everyone)
        ?? new RestRole(Client, this, 0) { Type = RoleType.Everyone };

    internal void Update(API.Rest.GetRoomRolesResponse model)
    {
        ImmutableDictionary<ulong, RestRole>.Builder roles =
            ImmutableDictionary.CreateBuilder<ulong, RestRole>();
        foreach (RoleModel roleModel in model.Roles)
            roles[roleModel.Id] = RestRole.Create(Client, this, roleModel);
        _roles = roles.ToImmutable();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(RequestOptions? options = null)
    {
        await RoomHelper.UpdateAsync(this, Client, options);
    }

    #region Channels

    /// <summary>
    ///     获取此房间内指定具有文字聊天能力的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    /// <remarks>
    ///     语音频道也是一种文字频道，此方法本意用于获取具有文字聊天能力的频道。如果通过此方法传入的 ID 对应的频道是语音频道，那么也会返回对应的语音频道实体。
    ///     如需获取频道的实际类型，请参考 <see cref="HeyBox.ChannelExtensions.GetChannelType(HeyBox.IChannel)"/>。
    /// </remarks>
    public async Task<RestTextChannel> GetTextChannelAsync(ulong id, RequestOptions? options = null) =>
        await RoomHelper.GetTextChannelAsync(this, Client, id, options);

    #endregion

    #region Roles

    /// <inheritdoc cref="HeyBox.IRoom.CreateRoleAsync(System.Action{RoleProperties},HeyBox.RequestOptions)" />
    public async Task<RestRole> CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options = null)
    {
        RoleModel model = await RoomHelper.CreateRoleAsync(this, Client, func, options);
        RestRole entity = RestRole.Create(Client, this, model);
        _roles = _roles.Add(entity.Id, entity);
        return entity;
    }

    #endregion

    #region Emotes

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<RoomEmote>> GetEmotesAsync(RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        return
        [
            ..model.Emojis.Select(x =>
            {
                RestRoomUser creator = RestRoomUser.Create(this, x.UserInfo);
                Meme meme = x.MemeInfo;
                return new RoomEmote(this, creator, meme.Name, meme.Path, meme.Extension, meme.CreateTime);
            })
        ];
    }

    /// <inheritdoc />
    public async Task<RoomEmote?> GetEmoteAsync(ulong id, RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        if (model.Emojis.FirstOrDefault(x => x.MemeInfo.Path == id) is not { } emoji)
            return null;
        RestRoomUser creator = RestRoomUser.Create(this, emoji.UserInfo);
        Meme meme = emoji.MemeInfo;
        return new RoomEmote(this, creator, meme.Name, meme.Path, meme.Extension, meme.CreateTime);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<RoomSticker>> GetStickersAsync(RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        return
        [
            ..model.Stickers.Select(x =>
            {
                RestRoomUser creator = RestRoomUser.Create(this, x.UserInfo);
                Meme meme = x.MemeInfo;
                return new RoomSticker(this, creator, meme.Name, meme.Path, meme.Extension, meme.CreateTime);
            })
        ];
    }

    /// <inheritdoc />
    public async Task<RoomSticker?> GetStickerAsync(ulong id, RequestOptions? options = null)
    {
        GetRoomMemesResponse model = await Client.ApiClient.GetRoomMemesAsync(Id, options);
        if (model.Stickers.FirstOrDefault(x => x.MemeInfo.Path == id) is not { } sticker)
            return null;
        RestRoomUser creator = RestRoomUser.Create(this, sticker.UserInfo);
        Meme meme = sticker.MemeInfo;
        return new RoomSticker(this, creator, meme.Name, meme.Path, meme.Extension, meme.CreateTime);
    }

    #endregion


    #region IRoom

    /// <inheritdoc />
    string? IRoom.Name => null;

    /// <inheritdoc />
    string? IRoom.Icon => null;

    /// <inheritdoc />
    IReadOnlyCollection<IRole> IRoom.Roles => Roles;

    /// <inheritdoc />
    IRole IRoom.EveryoneRole => EveryoneRole;

    /// <inheritdoc />
    IRole? IRoom.GetRole(ulong id) => id == 0 ? EveryoneRole : null;

    /// <inheritdoc />
    async Task<IRole> IRoom.CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options) =>
        await CreateRoleAsync(func, options).ConfigureAwait(false);

    /// <inheritdoc />
    async Task<ITextChannel?> IRoom.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetTextChannelAsync(id, options).ConfigureAwait(false) : null;

    #endregion
}
