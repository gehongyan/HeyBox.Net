namespace HeyBox.Interactions;

/// <summary>
///     要求调用命令的用户为指定用户。
/// </summary>
public class RequireUserAttribute : PreconditionAttribute
{
    /// <summary>
    ///     获取此先决条件指定的用户 ID。
    /// </summary>
    public uint UserId { get; }

    /// <summary>
    ///     要求调用命令的用户为特定用户。
    /// </summary>
    /// <param name="userId"> 需要用户拥有的用户 ID。 </param>
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
