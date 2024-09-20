namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的当前登录的用户信息。
/// </summary>
public class RestSelfUser : RestUser, ISelfUser
{
    internal RestSelfUser(BaseHeyBoxClient client, uint id)
        : base(client, id)
    {
    }
}
