namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的当前登录的用户信息。
/// </summary>
public class SocketSelfUser : SocketUser, ISelfUser
{
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

    internal override SocketGlobalUser GlobalUser { get; }

    internal SocketSelfUser(HeyBoxSocketClient client, SocketGlobalUser globalUser)
        : base(client, globalUser.Id)
    {
        GlobalUser = globalUser;
    }

    internal static SocketSelfUser Create(HeyBoxSocketClient client, ClientState state, uint id)
    {
        SocketSelfUser entity = new(client, client.GetOrCreateSelfUser(state, id));
        return entity;
    }
}
