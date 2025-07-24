namespace HeyBox.Interactions;

/// <summary>
///     包含与命令相关的结果信息。
/// </summary>
public interface IResult
{
    /// <summary>
    ///     获取操作过程中可能发生的错误类型。
    /// </summary>
    /// <returns>
    ///     一个 <see cref="InteractionCommandError" />，指示操作过程中可能发生的错误类型；
    ///     如果操作成功，则为 <see langword="null"/>。
    /// </returns>
    InteractionCommandError? Error { get; }

    /// <summary>
    ///     获取错误原因。
    /// </summary>
    /// <returns>
    ///     包含错误原因的字符串。
    /// </returns>
    string? ErrorReason { get; }

    /// <summary>
    ///     指示操作是否成功。
    /// </summary>
    /// <returns>
    ///     如果结果为正，则为 <see langword="true"/>；否则为 <see langword="false"/>。
    /// </returns>
    bool IsSuccess { get; }
}
