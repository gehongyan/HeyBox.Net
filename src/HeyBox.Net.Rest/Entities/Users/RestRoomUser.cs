namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的房间用户。
/// </summary>
public class RestRoomUser : RestUser, IRoomUser
{
    /// <inheritdoc />
    public string? Nickname { get; private set; }

    /// <inheritdoc />
    public string? DisplayName { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<ulong> RoleIds { get; private set; }

    /// <inheritdoc />
    public IRoom Room { get; }

    /// <inheritdoc />
    public ulong RoomId => Room.Id;

    /// <inheritdoc />
    internal RestRoomUser(RestRoom room, uint id)
        : base(room.Client, id)
    {
        Room = room;
        RoleIds = [];
    }

    internal static RestRoomUser Create(RestRoom room, API.RoomUser model)
    {
        RestRoomUser entity = new(room, model.UserId);
        entity.Update(model);
        return entity;
    }

    /// <inheritdoc />
    internal override void Update(API.RoomUser model)
    {
        base.Update(model);
        Nickname = string.IsNullOrEmpty(model.RoomNickname) ? null : model.RoomNickname;
        if (model.Roles is not null)
            RoleIds = [..model.Roles];
    }

    internal void AddRole(ulong roleId) => RoleIds = [..RoleIds, roleId];

    internal void RemoveRole(ulong roleId) => RoleIds = [..RoleIds.Where(x => x != roleId)];

    /// <inheritdoc />
    public Task AddRoleAsync(ulong roleId, RequestOptions? options = null) =>
        AddRolesAsync([roleId], options);

    /// <inheritdoc />
    public Task AddRoleAsync(IRole role, RequestOptions? options = null) =>
        AddRoleAsync(role.Id, options);

    /// <inheritdoc />
    public Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null) =>
        UserHelper.AddRolesAsync(this, Client, roleIds, options);

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
        UserHelper.RemoveRolesAsync(this, Client, roleIds, options);

    /// <inheritdoc />
    public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) =>
        RemoveRolesAsync(roles.Select(x => x.Id), options);

}
