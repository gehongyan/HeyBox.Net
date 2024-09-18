namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的身份组。
/// </summary>
public class RestRole : RestEntity<ulong>, IRole
{
    /// <inheritdoc />
    public IRoom Room { get; }

    /// <summary>
    ///     获取此角色是否为 <c>@全体成员</c> 全体成员角色。
    /// </summary>
    public bool IsEveryone => Id == 0;

    /// <inheritdoc />
    public string Mention => IsEveryone ? "@{all}" : MentionUtils.MentionRole(Id);

    internal RestRole(BaseHeyBoxClient heyBox, IRoom room, uint id)
        : base(heyBox, id)
    {
        Room = room;
    }
}
