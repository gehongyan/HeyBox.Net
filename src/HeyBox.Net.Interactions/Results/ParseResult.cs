using System.Diagnostics.CodeAnalysis;

namespace HeyBox.Interactions;

/// <summary>
///     表示一个解析结果。
/// </summary>
public struct ParseResult : IResult
{
    /// <summary>
    ///     获取解析到的参数。
    /// </summary>
    public object?[]? Args { get; }

    /// <summary>
    ///     获取解析的错误。
    /// </summary>
    public InteractionCommandError? Error { get; }

    /// <summary>
    ///     获取解析错误的原因。
    /// </summary>
    public string? ErrorReason { get; }

    /// <inheritdoc />
    public bool IsSuccess => !Error.HasValue;

    private ParseResult(object?[]? args, InteractionCommandError? error, string? reason)
    {
        Args = args;
        Error = error;
        ErrorReason = reason;
    }

    /// <summary>
    ///     初始化一个 <see cref="ParseResult" /> 结构体，表示一个成功的解析结果。
    /// </summary>
    /// <param name="args"> 解析的参数。 </param>
    /// <returns></returns>
    public static ParseResult FromSuccess(object?[] args) =>
        new ParseResult(args, null, null);

    /// <summary>
    ///     初始化一个 <see cref="ParseResult" /> 结构体，表示一个解析失败的结果。
    /// </summary>
    /// <param name="exception"> 解析失败的异常。 </param>
    /// <returns> 一个表示解析失败的结果。 </returns>
    public static ParseResult FromError(Exception exception) =>
        new ParseResult(null, InteractionCommandError.Exception, exception.Message);

    /// <summary>
    ///     初始化一个 <see cref="ParseResult" /> 结构体，表示一个解析失败的结果。
    /// </summary>
    /// <param name="error"> 解析失败的错误类型。 </param>
    /// <param name="reason"> 解析失败的原因。 </param>
    /// <returns> 一个表示解析失败的结果。 </returns>
    public static ParseResult FromError(InteractionCommandError error, string reason) =>
        new ParseResult(null, error, reason);

    /// <summary>
    ///     从一个 <see cref="IResult" /> 实例创建一个 <see cref="ParseResult" /> 实例。
    /// </summary>
    /// <param name="result"> 要创建的 <see cref="ParseResult" /> 实例的 <see cref="IResult" /> 实例。 </param>
    /// <returns> 一个 <see cref="ParseResult" /> 实例。 </returns>
    public static ParseResult FromError(IResult result) =>
        new ParseResult(null, result.Error, result.ErrorReason);

    /// <inheritdoc />
    public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
}
