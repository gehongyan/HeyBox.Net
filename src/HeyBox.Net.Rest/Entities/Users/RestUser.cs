namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的用户。
/// </summary>
public class RestUser : RestEntity<uint>, IUser
{
    /// <inheritdoc />
    public string? Username { get; private set; }

    /// <inheritdoc />
    public bool? IsBot { get; private set; }

    /// <inheritdoc />
    public string? Avatar { get; private set; }

    /// <inheritdoc />
    public string? AvatarDecorationType { get; private set; }

    /// <inheritdoc />
    public string? AvatarDecorationUrl { get; private set; }

    /// <inheritdoc />
    public int? Level { get; private set; }

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionUser(Id);

    internal RestUser(BaseHeyBoxClient client, uint id)
        : base(client, id)
    {
    }

    internal static RestUser Create(BaseHeyBoxClient client, uint id)
    {
        RestUser entity = new(client, id);
        return entity;
    }

    internal virtual void Update(API.RoomUser model)
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
