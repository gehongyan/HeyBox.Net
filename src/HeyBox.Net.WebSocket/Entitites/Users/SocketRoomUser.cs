using System.Collections.Immutable;
using Model = HeyBox.API.Gateway.SenderInfo;
using UserModel = HeyBox.API.Gateway.SenderInfo;

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
    public IReadOnlyCollection<SocketRole> Roles => [.._roleIds.Select(x => Room.AddOrUpdateRole(x))];

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

    internal static SocketRoomUser Create(SocketRoom room, ClientState state, UserModel model)
    {
        SocketRoomUser entity = new(room, room.Client.GetOrCreateUser(state, model));
        entity.Update(state, model);
        return entity;
    }

    /// <inheritdoc />
    internal override void Update(ClientState state, Model model)
    {
        base.Update(state, model);
        Nickname = string.IsNullOrEmpty(model.RoomNickname) ? null : model.RoomNickname;
        _roleIds = [..model.Roles];
    }

    #region IRoomUser

    /// <inheritdoc />
    IRoom IRoomUser.Room => Room;

    /// <inheritdoc />
    ulong IRoomUser.RoomId => Room.Id;

    /// <inheritdoc />
    IReadOnlyCollection<ulong> IRoomUser.RoleIds => _roleIds;

    #endregion
}
