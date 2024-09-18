namespace HeyBox;

/// <summary>
///     表示一个通用的身份组。
/// </summary>
public interface IRole : IEntity<ulong>, IMentionable
{
    /// <summary>
    ///     获取拥有此角色的房间。
    /// </summary>
    IRoom Room { get; }
}
