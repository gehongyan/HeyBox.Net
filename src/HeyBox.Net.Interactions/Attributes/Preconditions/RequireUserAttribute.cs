namespace HeyBox.Interactions;

/// <summary>
///     Requires the user invoking the command to have a specified user.
/// </summary>
public class RequireUserAttribute : PreconditionAttribute
{
    /// <summary>
    ///     Gets the specified User ID of the precondition.
    /// </summary>
    public uint UserId { get; }

    /// <summary>
    ///     Requires that the user invoking the command to have a specific Role.
    /// </summary>
    /// <param name="userId">Id of the role that the user must have.</param>
    public RequireUserAttribute(uint userId)
    {
        UserId = userId;
    }

    /// <inheritdoc />
    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider? services)
    {
        return Task.FromResult(context.User?.Id == UserId
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError(ErrorMessage ?? $"User requires room user ID {UserId}."));
    }
}
