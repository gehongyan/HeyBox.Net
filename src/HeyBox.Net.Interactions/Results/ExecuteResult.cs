namespace HeyBox.Interactions;

/// <summary>
///     表示命令整体执行结果的信息。
/// </summary>
public struct ExecuteResult : IResult
{
    /// <summary>
    ///     获取命令执行过程中可能发生的异常。
    /// </summary>
    public Exception? Exception { get; }

    /// <inheritdoc/>
    public InteractionCommandError? Error { get; }

    /// <inheritdoc/>
    public string? ErrorReason { get; }

    /// <inheritdoc/>
    public bool IsSuccess => !Error.HasValue;

    private ExecuteResult(Exception? exception, InteractionCommandError? commandError, string? errorReason)
    {
        Exception = exception;
        Error = commandError;
        ErrorReason = errorReason;
    }

    /// <summary>
    ///     初始化一个新的 <see cref="ExecuteResult" />，没有错误，表示执行成功。
    /// </summary>
    /// <returns>
    ///     一个不包含任何错误的 <see cref="ExecuteResult" />。
    /// </returns>
    public static ExecuteResult FromSuccess() =>
        new ExecuteResult(null, null, null);

    /// <summary>
    ///     用指定的 <see cref="InteractionCommandError" /> 及其原因初始化一个新的 <see cref="ExecuteResult" />，
    ///     表示执行不成功。
    /// </summary>
    /// <param name="commandError">错误类型。</param>
    /// <param name="reason">错误原因。</param>
    /// <returns>
    ///     一个包含 <see cref="InteractionCommandError" /> 和原因的 <see cref="ExecuteResult" />。
    /// </returns>
    public static ExecuteResult FromError(InteractionCommandError commandError, string reason) =>
        new ExecuteResult(null, commandError, reason);

    /// <summary>
    ///     用指定的异常初始化一个新的 <see cref="ExecuteResult" />，表示执行不成功。
    /// </summary>
    /// <param name="exception">导致命令执行失败的异常。</param>
    /// <returns>
    ///     一个包含导致执行不成功的异常的 <see cref="ExecuteResult" />，以及类型为 <c>Exception</c> 的
    ///     <see cref="InteractionCommandError" /> 和异常消息作为原因。
    /// </returns>
    public static ExecuteResult FromError(Exception exception) =>
        new ExecuteResult(exception, InteractionCommandError.Exception, exception.Message);

    /// <summary>
    ///     用指定的结果初始化一个新的 <see cref="ExecuteResult" />；这可能是成功的执行，也可能不是，
    ///     具体取决于 <see cref="IResult.Error" /> 和 <see cref="IResult.ErrorReason" /> 的值。
    /// </summary>
    /// <param name="result"> 要继承的结果。 </param>
    /// <returns>
    ///     一个继承了 <see cref="IResult"/> 错误类型和原因的 <see cref="ExecuteResult"/>。
    /// </returns>
    public static ExecuteResult FromError(IResult result) =>
        new ExecuteResult(null, result.Error, result.ErrorReason);

    /// <summary>
    ///     获取指示执行结果的字符串。
    /// </summary>
    /// <returns>
    ///     如果 <see cref="IsSuccess"/> 为 <see langword="true"/>，则为 <c>Success</c>；否则为 "<see cref="Error"/>:
    ///     <see cref="ErrorReason"/>"。
    /// </returns>
    public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
}
