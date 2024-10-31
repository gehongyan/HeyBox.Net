using System.Collections.Immutable;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的房间用户。
/// </summary>
public class SocketRoomUser : SocketUser, IRoomUser
{
    private ImmutableArray<ulong> _roleIds;

    internal override SocketGlobalUser GlobalUser { get; }

    /// <inheritdoc cref="HeyBox.IRoomUser.Room" />
    public SocketRoom Room { get; }

    /// <inheritdoc />
    public string? DisplayName => Nickname ?? Username;

    /// <inheritdoc />
    public string? Nickname { get; private set; }

    /// <inheritdoc />
    public override string? Username
    {
        get => GlobalUser.Username;
        internal set => GlobalUser.Username = value;
    }

    /// <inheritdoc />
    public override bool? IsBot
    {
        get => GlobalUser.IsBot;
        internal set => GlobalUser.IsBot = value;
    }

    /// <inheritdoc />
    public override string? Avatar
    {
        get => GlobalUser.Avatar;
        internal set => GlobalUser.Avatar = value;
    }

    /// <inheritdoc />
    public override string? AvatarDecorationType
    {
        get => GlobalUser.AvatarDecorationType;
        internal set => GlobalUser.AvatarDecorationType = value;
    }

    /// <inheritdoc />
    public override string? AvatarDecorationUrl
    {
        get => GlobalUser.AvatarDecorationUrl;
        internal set => GlobalUser.AvatarDecorationUrl = value;
    }

    /// <inheritdoc />
    public override int? Level
    {
        get => GlobalUser.Level;
        internal set => GlobalUser.Level = value;
    }

    /// <summary>
    ///     获取此用户在该房间内拥有的所有角色。
    /// </summary>
    public IReadOnlyCollection<SocketRole> Roles => [.._roleIds.Select(x => Room.GetRole(x)).OfType<SocketRole>()];

    /// <inheritdoc />
    internal SocketRoomUser(SocketRoom room, SocketGlobalUser globalUser)
        : base(room.Client, globalUser.Id)
    {
        _roleIds = [];
        Room = room;
        GlobalUser = globalUser;
    }

    internal static SocketRoomUser Create(SocketRoom room, ClientState state, uint id)
    {
        SocketRoomUser entity = new(room, room.Client.GetOrCreateUser(state, id));
        return entity;
    }

    internal static SocketRoomUser Create(SocketRoom room, ClientState state, API.RoomUser model)
    {
        SocketRoomUser entity = new(room, room.Client.GetOrCreateUser(state, model));
        entity.Update(state, model);
        return entity;
    }

    /// <inheritdoc />
    internal override void Update(ClientState state, API.RoomUser model)
    {
        base.Update(state, model);
        Nickname = string.IsNullOrEmpty(model.RoomNickname) ? null : model.RoomNickname;
        if (model.Roles is not null)
            _roleIds = [..model.Roles];
    }

    internal void AddRole(ulong roleId)
    {
        _roleIds = [.._roleIds, roleId];
    }

    internal void RemoveRole(ulong roleId)
    {
        _roleIds = _roleIds.Remove(roleId);
    }

    /// <inheritdoc />
    public Task AddRoleAsync(ulong roleId, RequestOptions? options = null) =>
        AddRolesAsync([roleId], options);

    /// <inheritdoc />
    public Task AddRoleAsync(IRole role, RequestOptions? options = null) =>
        AddRoleAsync(role.Id, options);

    /// <inheritdoc />
    public Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null) =>
        SocketUserHelper.AddRolesAsync(this, Client, roleIds, options);

    /// <inheritdoc />
    public Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) =>
        AddRolesAsync(roles.Select(x => x.Id), options);

    /// <inheritdoc />
    public Task RemoveRoleAsync(ulong roleId, RequestOptions? options = null) =>
        RemoveRolesAsync([roleId], options);

    /// <inheritdoc />
    public Task RemoveRoleAsync(IRole role, RequestOptions? options = null) =>
        RemoveRoleAsync(role.Id, options);

    /// <inheritdoc />
    public Task RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null) =>
        SocketUserHelper.RemoveRolesAsync(this, Client, roleIds, options);

    /// <inheritdoc />
    public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) =>
        RemoveRolesAsync(roles.Select(x => x.Id), options);

    #region IRoomUser

    /// <inheritdoc />
    IRoom IRoomUser.Room => Room;

    /// <inheritdoc />
    ulong IRoomUser.RoomId => Room.Id;

    /// <inheritdoc />
    IReadOnlyCollection<ulong> IRoomUser.RoleIds => _roleIds;

    #endregion
}
