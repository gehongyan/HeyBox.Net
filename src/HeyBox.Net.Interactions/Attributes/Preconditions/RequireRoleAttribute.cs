namespace HeyBox.Interactions;

/// <summary>
///     要求调用命令的用户拥有指定的角色。
/// </summary>
public class RequireRoleAttribute : PreconditionAttribute
{
    /// <summary>
    ///     获取此先决条件指定的角色 ID。
    /// </summary>
    public ulong RoleId { get; }

    /// <summary>
    ///     获取或设置如果由于在非群组频道运行而导致先决条件失败时的错误消息。
    /// </summary>
    public string? NotAGuildErrorMessage { get; set; }

    /// <summary>
    ///     要求调用命令的用户拥有特定角色。
    /// </summary>
    /// <param name="roleId"> 需要用户拥有的角色 ID。 </param>
    public RequireRoleAttribute(ulong roleId)
    {
        RoleId = roleId;
    }

    /// <inheritdoc />
    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider? services)
    {
        if (context.User is not IRoomUser roomUser)
            return Task.FromResult(PreconditionResult.FromError(NotAGuildErrorMessage ?? "Command must be used in a guild channel."));

        return Task.FromResult(roomUser.RoleIds.Contains(RoleId)
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError(ErrorMessage ?? $"User requires room role ID {RoleId}."));
    }
}
