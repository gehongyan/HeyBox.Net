namespace HeyBox.Interactions;

/// <summary>
///     Requires the user invoking the command to have a specified role.
/// </summary>
public class RequireRoleAttribute : PreconditionAttribute
{
    /// <summary>
    ///     Gets the specified Role ID of the precondition.
    /// </summary>
    public ulong RoleId { get; }

    /// <summary>
    ///     Gets or sets the error message if the precondition
    ///     fails due to being run outside of a Guild channel.
    /// </summary>
    public string? NotAGuildErrorMessage { get; set; }

    /// <summary>
    ///     Requires that the user invoking the command to have a specific Role.
    /// </summary>
    /// <param name="roleId">Id of the role that the user must have.</param>
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
