using System.Diagnostics.CodeAnalysis;

namespace HeyBox.Interactions;

/// <summary> 包含命令搜索的信息。 </summary>
/// <typeparam name="T"> 目标命令类型。 </typeparam>
public struct SearchResult<T> : IResult where T : class, ICommandInfo
{
    /// <summary> 获取命令搜索的输入文本。 </summary>
    public string? Text { get; }

    /// <summary> 获取搜索成功时找到的命令。 </summary>
    public T? Command { get; }

    /// <summary> 获取通配符模式捕获的正则分组。 </summary>
    public string[]? RegexCaptureGroups { get; }

    /// <inheritdoc/>
    public InteractionCommandError? Error { get; }

    /// <inheritdoc/>
    public string? ErrorReason { get; }

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Command))]
    public bool IsSuccess => !Error.HasValue;

    private SearchResult(string? text, T? commandInfo, string[]? captureGroups, InteractionCommandError? error, string? reason)
    {
        Text = text;
        Error = error;
        RegexCaptureGroups = captureGroups;
        Command = commandInfo;
        ErrorReason = reason;
    }

    /// <summary> 初始化一个新的 <see cref="SearchResult{T}" />，无错误，表示执行成功。 </summary>
    /// <returns> 一个不包含任何错误的 <see cref="SearchResult{T}" />。 </returns>
    public static SearchResult<T> FromSuccess(string text, T commandInfo, string[]? wildCardMatch = null) =>
        new SearchResult<T>(text, commandInfo, wildCardMatch, null, null);

    /// <summary> 初始化一个新的 <see cref="SearchResult{T}" />，带有指定的 <see cref="InteractionCommandError" /> 及原因，表示执行失败。 </summary>
    /// <param name="text"> 命令搜索的输入文本。 </param>
    /// <param name="error"> 错误类型。 </param>
    /// <param name="reason"> 错误原因。 </param>
    /// <returns> 一个包含 <see cref="InteractionCommandError" /> 和原因的 <see cref="SearchResult{T}" />。 </returns>
    public static SearchResult<T> FromError(string text, InteractionCommandError error, string reason) =>
        new SearchResult<T>(text, null, null, error, reason);

    /// <summary> 初始化一个新的 <see cref="SearchResult{T}" />，带有指定异常，表示执行失败。 </summary>
    /// <param name="ex"> 导致命令执行失败的异常。 </param>
    /// <returns> 一个包含导致执行失败异常的 <see cref="SearchResult{T}" />，以及类型为 <c>Exception</c> 的 <see cref="InteractionCommandError" /> 和异常消息作为原因。 </returns>
    public static SearchResult<T> FromError(Exception ex) =>
        new SearchResult<T>(null, null, null, InteractionCommandError.Exception, ex.Message);

    /// <summary> 初始化一个新的 <see cref="SearchResult{T}" />，带有指定结果，是否成功取决于 <see cref="IResult.Error" /> 和 <see cref="IResult.ErrorReason" />。 </summary>
    /// <param name="result"> 要继承的结果。 </param>
    /// <returns> 一个继承了 <see cref="IResult"/> 错误类型和原因的 <see cref="SearchResult{T}"/>。 </returns>
    public static SearchResult<T> FromError(IResult result) =>
        new SearchResult<T>(null, null, null, result.Error, result.ErrorReason);

    /// <inheritdoc/>
    public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
}
