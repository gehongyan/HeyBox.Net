namespace HeyBox.Interactions;

/// <summary>
///     Represents a result type for command preconditions.
/// </summary>
public class PreconditionResult : IResult
{
    /// <inheritdoc/>
    public InteractionCommandError? Error { get; }

    /// <inheritdoc/>
    public string? ErrorReason { get; }

    /// <inheritdoc/>
    public bool IsSuccess => Error == null;

    /// <summary>
    ///     Initializes a new <see cref="PreconditionResult" /> class with the command <paramref name="error"/> type
    ///     and reason.
    /// </summary>
    /// <param name="error">The type of failure.</param>
    /// <param name="reason">The reason of failure.</param>
    protected PreconditionResult(InteractionCommandError? error, string? reason)
    {
        Error = error;
        ErrorReason = reason;
    }

    /// <summary>
    ///     获取一个表示成功的 <see cref="PreconditionResult" /> 实例。
    /// </summary>
    public static PreconditionResult FromSuccess() =>
        new PreconditionResult(null, null);

    /// <summary>
    ///     获取一个表示失败的 <see cref="PreconditionResult" /> 实例。
    /// </summary>
    /// <param name="exception"> 导致先决条件检查失败的异常。 </param>
    public static PreconditionResult FromError(Exception exception) =>
        new PreconditionResult(InteractionCommandError.Exception, exception.Message);

    /// <summary>
    ///     获取一个表示失败的 <see cref="PreconditionResult" /> 实例。
    /// </summary>
    /// <param name="result"> 包含错误信息的结果。 </param>
    public static PreconditionResult FromError(IResult result) =>
        new PreconditionResult(result.Error, result.ErrorReason);

    /// <summary>
    ///     获取一个表示失败的 <see cref="PreconditionResult" /> 实例。
    /// </summary>
    /// <param name="reason"> 先决条件检查失败的原因。 </param>
    public static PreconditionResult FromError(string reason) =>
        new PreconditionResult(InteractionCommandError.UnmetPrecondition, reason);
}
