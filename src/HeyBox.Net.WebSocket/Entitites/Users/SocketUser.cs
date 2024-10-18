using HeyBox.API.Gateway;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的用户。
/// </summary>
public abstract class SocketUser : SocketEntity<uint>, IUser
{
    /// <inheritdoc />
    public abstract string? Username { get; internal set; }

    /// <inheritdoc />
    public abstract bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public abstract string? Avatar { get; internal set; }

    /// <inheritdoc />
    public abstract string? AvatarDecorationType { get; internal set; }

    /// <inheritdoc />
    public abstract string? AvatarDecorationUrl { get; internal set; }

    /// <inheritdoc />
    public abstract int? Level { get; internal set; }

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionUser(Id);

    internal abstract SocketGlobalUser GlobalUser { get; }

    internal SocketUser(HeyBoxSocketClient client, uint id)
        : base(client, id)
    {
    }

    internal virtual void Update(ClientState state, API.RoomUser model)
    {
        Username = model.Nickname;
        IsBot = model.Bot;
        Avatar = model.Avatar;
        AvatarDecorationType = model.AvatarDecoration.SourceType;
        AvatarDecorationUrl = model.AvatarDecoration.SourceUrl;
        Level = model.Level;

        IsPopulated = true;
    }

}
