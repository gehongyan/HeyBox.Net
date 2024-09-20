namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的用户。
/// </summary>
public class RestUser : RestEntity<uint>, IUser
{
    /// <inheritdoc />
    string IMentionable.Mention => MentionUtils.MentionUser(Id);

    internal RestUser(BaseHeyBoxClient client, uint id)
        : base(client, id)
    {
    }

    internal static RestUser Create(BaseHeyBoxClient client, uint id)
    {
        RestUser entity = new(client, id);
        return entity;
    }

    #region IUser

    /// <inheritdoc />
    string? IUser.Username => null;

    /// <inheritdoc />
    bool? IUser.IsBot => null;

    /// <inheritdoc />
    string? IUser.Avatar => null;

    /// <inheritdoc />
    string? IUser.AvatarDecorationType => null;

    /// <inheritdoc />
    string? IUser.AvatarDecorationUrl => null;

    /// <inheritdoc />
    int? IUser.Level => null;

    #endregion
}
