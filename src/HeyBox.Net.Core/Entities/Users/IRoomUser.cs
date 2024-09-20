namespace HeyBox;

/// <summary>
///     表示一个通用的房间用户。
/// </summary>
public interface IRoomUser : IUser
{
    /// <summary>
    ///     获取此用户在该房间内的昵称。
    /// </summary>
    /// <remarks>
    ///     如果此用户在该房间内没有设置昵称，则此属性返回 <see langword="null"/>。
    /// </remarks>
    string? Nickname { get; }

    /// <summary>
    ///     获取此用户的显示名称。
    /// </summary>
    /// <remarks>
    ///     如果此用户在该房间内设置了昵称，则此属性返回昵称；否则返回用户名。
    /// </remarks>
    string? DisplayName { get; }

    /// <summary>
    ///     获取此用户在该房间内拥有的所有角色的 ID。
    /// </summary>
    IReadOnlyCollection<ulong> RoleIds { get; }

    /// <summary>
    ///     获取此房间用户所属的房间。
    /// </summary>
    IRoom Room { get; }

    /// <summary>
    ///     获取此用户所属房间的 ID。
    /// </summary>
    ulong RoomId { get; }
}
