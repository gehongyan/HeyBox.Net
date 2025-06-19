namespace HeyBox.Interactions;

/// <summary>
///     表示可被执行的命令信息对象。
/// </summary>
public interface ICommandInfo
{
    /// <summary>
    ///     获取命令的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取命令处理方法的名称。
    /// </summary>
    string MethodName { get; }

    /// <summary>
    ///     获取该方法所属的模块。
    /// </summary>
    ModuleInfo Module { get; }

    /// <summary>
    ///     获取底层命令服务。
    /// </summary>
    InteractionService CommandService { get; }

    /// <summary>
    ///     获取此命令的运行模式。
    /// </summary>
    RunMode RunMode { get; }

    /// <summary>
    ///     获取此命令的特性集合。
    /// </summary>
    IReadOnlyCollection<Attribute> Attributes { get; }

    /// <summary>
    ///     获取此命令的先决条件集合。
    /// </summary>
    IReadOnlyCollection<PreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     获取此命令的参数集合。
    /// </summary>
    IReadOnlyCollection<IParameterInfo> Parameters { get; }

    /// <summary>
    ///     获取命令名称是否应被视为正则表达式。
    /// </summary>
    bool TreatNameAsRegex { get; }

    /// <summary>
    ///     使用提供的上下文执行命令。
    /// </summary>
    /// <param name="context"> 执行上下文。 </param>
    /// <param name="services"> 用于创建模块实例的依赖项。 </param>
    /// <returns>
    ///     表示执行过程的任务。任务结果包含执行结果。
    /// </returns>
    Task<IResult> ExecuteAsync(IInteractionContext context, IServiceProvider services);

    /// <summary>
    ///     检查执行上下文是否满足命令的先决条件要求。
    /// </summary>
    Task<PreconditionResult> CheckPreconditionsAsync(IInteractionContext context, IServiceProvider services);
}
