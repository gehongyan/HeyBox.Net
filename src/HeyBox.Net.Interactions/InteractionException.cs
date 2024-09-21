namespace HeyBox.Interactions;

/// <summary>
///     表示一个交互执行过程中发生的异常。
/// </summary>
public class InteractionException : Exception
{
    /// <summary>
    ///     获取引发异常的命令信息。
    /// </summary>
    public ICommandInfo CommandInfo { get; }

    /// <summary>
    ///     获取引发异常的交互上下文。
    /// </summary>
    public IInteractionContext InteractionContext { get; }

    /// <summary>
    ///     初始化一个 <see cref="InteractionException"/> 类的新实例。
    /// </summary>
    /// <param name="commandInfo"> 引发异常的命令信息。 </param>
    /// <param name="context"> 引发异常的交互上下文。 </param>
    /// <param name="exception"> 异常信息。 </param>
    public InteractionException(ICommandInfo commandInfo, IInteractionContext context, Exception exception)
        : base($"Error occurred executing {commandInfo}.", exception)
    {
        CommandInfo = commandInfo;
        InteractionContext = context;
    }
}
