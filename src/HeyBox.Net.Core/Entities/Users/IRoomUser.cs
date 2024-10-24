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

    #region Roles

    /// <summary>
    ///     在该房间内授予此用户指定的角色。
    /// </summary>
    /// <param name="roleId"> 要在该房间内为此用户授予的角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRoleAsync(ulong roleId, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内授予此用户指定的角色。
    /// </summary>
    /// <param name="role"> 要在该房间内为此用户授予的角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRoleAsync(IRole role, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内授予此用户指定的一些角色。
    /// </summary>
    /// <param name="roleIds"> 要在该房间内为此用户授予的所有角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内授予此用户指定的一些角色。
    /// </summary>
    /// <param name="roles"> 要在该房间内为此用户授予的所有角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内撤销此用户指定的角色。
    /// </summary>
    /// <param name="roleId"> 要在该房间内为此用户撤销的角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRoleAsync(ulong roleId, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内撤销此用户指定的角色。
    /// </summary>
    /// <param name="role"> 要在该房间内为此用户撤销的角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRoleAsync(IRole role, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内撤销此用户指定的一些角色。
    /// </summary>
    /// <param name="roleIds"> 要在该房间内为此用户撤销的所有角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null);

    /// <summary>
    ///     在该房间内撤销此用户指定的一些角色。
    /// </summary>
    /// <param name="roles"> 要在该房间内为此用户撤销的所有角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null);

    #endregion
}
