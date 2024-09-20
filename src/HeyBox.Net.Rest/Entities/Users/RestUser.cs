namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的用户。
/// </summary>
public class RestUser : RestEntity<uint>, IUser
{
    /// <inheritdoc />
    public string? Username { get; internal set; }

    /// <inheritdoc />
    public bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public string? Avatar { get; internal set; }

    /// <inheritdoc />
    public string? AvatarDecorationType { get; internal set; }

    /// <inheritdoc />
    public string? AvatarDecorationUrl { get; internal set; }

    /// <inheritdoc />
    public int? Level { get; internal set; }

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
}
