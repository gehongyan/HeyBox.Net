namespace HeyBox;

/// <summary>
///     表示一个为角色设置的频道权限重写设置。
/// </summary>
public class RolePermissionOverwrite : IPermissionOverwrite<IRole, ulong>
{
    /// <summary>
    ///     获取此重写所应用的角色的。
    /// </summary>
    public IRole? Target { get; }

    /// <summary>
    ///     获取此重写所应用的角色的 ID。
    /// </summary>
    public ulong TargetId { get; }

    /// <inheritdoc />
    public PermissionOverwriteTarget TargetType => PermissionOverwriteTarget.Role;

    /// <inheritdoc />
    public OverwritePermissions Permissions { get; }

    internal RolePermissionOverwrite(ulong targetId, OverwritePermissions permissions)
    {
        TargetId = targetId;
        Permissions = permissions;
    }

    internal RolePermissionOverwrite(IRole target, OverwritePermissions permissions)
        : this(target.Id, permissions)
    {
        Target = target;
    }

    internal RolePermissionOverwrite(ulong targetId, IRole? target, OverwritePermissions permissions)
        : this(targetId, permissions)
    {
        Target = target;
    }
}
