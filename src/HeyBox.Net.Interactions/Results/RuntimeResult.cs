namespace HeyBox.Interactions;

/// <summary> 表示用于创建命令结果容器的基类。 </summary>
public abstract class RuntimeResult : IResult
{
    /// <inheritdoc/>
    public InteractionCommandError? Error { get; }

    /// <inheritdoc/>
    public string? ErrorReason { get; }

    /// <inheritdoc/>
    public bool IsSuccess => !Error.HasValue;

    /// <summary> 使用错误类型和原因初始化 <see cref="RuntimeResult" /> 类的新实例。 </summary>
    /// <param name="error"> 失败类型，若无则为 <see langword="null" />。 </param>
    /// <param name="reason"> 失败原因。 </param>
    protected RuntimeResult(InteractionCommandError? error, string? reason)
    {
        Error = error;
        ErrorReason = reason;
    }

    /// <summary> 获取指示运行时结果的字符串。 </summary>
    /// <returns>
    ///     如果 <see cref="IsSuccess"/> 为 <see langword="true" />，则为 <c>Success</c>；否则为 "<see cref="Error"/>: <see cref="ErrorReason"/>"。
    /// </returns>
    public override string ToString() => ErrorReason ?? (IsSuccess ? "Successful" : "Unsuccessful");
}
