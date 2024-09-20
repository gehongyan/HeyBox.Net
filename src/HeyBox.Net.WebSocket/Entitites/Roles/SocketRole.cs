namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的身份组。
/// </summary>
public class SocketRole : SocketEntity<ulong>, IRole
{
    /// <inheritdoc />
    public IRoom Room { get; }

    /// <summary>
    ///     获取此角色是否为 <c>@全体成员</c> 全体成员角色。
    /// </summary>
    public bool IsEveryone => Id == 0;

    /// <inheritdoc />
    public string Mention => IsEveryone ? "@{all}" : MentionUtils.MentionRole(Id);

    internal SocketRole(SocketRoom room, ulong id)
        : base(room.Client, id)
    {
        Room = room;
    }
}
