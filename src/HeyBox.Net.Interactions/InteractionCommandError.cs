namespace HeyBox.Interactions;

/// <summary>
///     定义命令可能抛出的错误类型。
/// </summary>
public enum InteractionCommandError
{
    /// <summary>
    ///     当命令未知时抛出。
    /// </summary>
    UnknownCommand,

    /// <summary>
    ///     当命令参数无法被类型读取器转换时抛出。
    /// </summary>
    ConvertFailed,

    /// <summary>
    ///     当输入文本参数过少或过多时抛出。
    /// </summary>
    BadArgs,

    /// <summary>
    ///     当命令执行过程中发生异常时抛出。
    /// </summary>
    Exception,

    /// <summary>
    ///     当命令在运行时未成功执行时抛出。
    /// </summary>
    Unsuccessful,

    /// <summary>
    ///     当命令未满足先决条件时抛出。
    /// </summary>
    UnmetPrecondition,

    /// <summary>
    ///     当命令上下文无法被 <see cref="ICommandInfo"/> 解析时抛出。
    /// </summary>
    ParseFailed
}
