namespace HeyBox.Interactions;

/// <summary>
///     表示命令执行工作流的模式。
/// </summary>
public enum RunMode
{
    /// <summary>
    ///     执行命令在与网关相同的线程上，命令阻塞网关线程执行直到完成。
    /// </summary>
    Sync,

    /// <summary>
    ///     在与网关不同的线程上执行命令，命令在后台执行。
    /// </summary>
    Async,

    /// <summary>
    ///     使用在 <see cref="HeyBox.Interactions.InteractionServiceConfig"/> 中设置的默认行为。
    /// </summary>
    Default
}
