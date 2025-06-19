using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace HeyBox.Interactions;

/// <summary>
///     表示缓存的方法执行委托。
/// </summary>
/// <param name="context"> 注入到模块类的执行上下文。 </param>
/// <param name="args"> 方法参数数组。 </param>
/// <param name="serviceProvider"> 用于初始化模块的服务集合。 </param>
/// <param name="commandInfo"> 被执行方法的命令信息类。 </param>
/// <returns>
///     表示执行操作的任务。
/// </returns>
public delegate Task ExecuteCallback(IInteractionContext context, object?[] args, IServiceProvider? serviceProvider, ICommandInfo commandInfo);

/// <summary>
///     <see cref="InteractionService"/> 命令的基础信息类。
/// </summary>
/// <typeparam name="TParameter"> 此命令类型所用的 <see cref="IParameterInfo"/> 类型。 </typeparam>
public abstract class CommandInfo<TParameter> : ICommandInfo where TParameter : class, IParameterInfo
{
    private readonly ExecuteCallback? _action;
    private readonly ILookup<string?, PreconditionAttribute> _groupedPreconditions;

    /// <inheritdoc/>
    public ModuleInfo Module { get; }

    /// <inheritdoc/>
    public InteractionService CommandService { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public string MethodName { get; }

    /// <inheritdoc/>
    public RunMode RunMode { get; }

    /// <inheritdoc/>
    public IReadOnlyCollection<Attribute> Attributes { get; }

    /// <inheritdoc/>
    public IReadOnlyCollection<PreconditionAttribute> Preconditions { get; }

    /// <inheritdoc cref="ICommandInfo.Parameters"/>
    public abstract IReadOnlyList<TParameter> Parameters { get; }

    /// <inheritdoc />
    public bool TreatNameAsRegex { get; }

    internal CommandInfo(Builders.ICommandBuilder builder, ModuleInfo module, InteractionService commandService)
    {
        CommandService = commandService;
        Module = module;

        Name = builder.Name ?? throw new InvalidOperationException("Command name must be specified.");
        MethodName = builder.MethodName ?? throw new InvalidOperationException("Method name must be specified.");
        RunMode = builder.RunMode != RunMode.Default ? builder.RunMode : commandService._runMode;
        Attributes = builder.Attributes.ToImmutableArray();
        Preconditions = builder.Preconditions.ToImmutableArray();
        TreatNameAsRegex = builder.TreatNameAsRegex;

        _action = builder.Callback;
        _groupedPreconditions = builder.Preconditions.ToLookup(x => x.Group, x => x, StringComparer.Ordinal);
    }

    /// <inheritdoc/>
    public virtual Task<IResult> ExecuteAsync(IInteractionContext context, IServiceProvider? services)
    {
        switch (RunMode)
        {
            case RunMode.Sync:
                return ExecuteInternalAsync(context, services);
            case RunMode.Async:
                _ = Task.Run(async () =>
                {
                    await ExecuteInternalAsync(context, services).ConfigureAwait(false);
                });
                break;
            default:
                throw new InvalidOperationException($"RunMode {RunMode} is not supported.");
        }

        return Task.FromResult((IResult)ExecuteResult.FromSuccess());
    }

    /// <summary>
    ///     解析此命令的参数。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <param name="services"> 用于依赖注入的服务集合。 </param>
    /// <returns> 表示异步解析操作的任务。任务结果包含解析后的参数。 </returns>
    protected abstract Task<IResult> ParseArgumentsAsync(IInteractionContext context, IServiceProvider? services);

    private async Task<IResult> ExecuteInternalAsync(IInteractionContext context, IServiceProvider? services)
    {
        await CommandService._cmdLogger.DebugAsync($"Executing {GetLogString(context)}").ConfigureAwait(false);

        using IServiceScope? scope = services?.CreateScope();

        if (CommandService._autoServiceScopes)
            services = scope?.ServiceProvider ?? EmptyServiceProvider.Instance;

        try
        {
            PreconditionResult preconditionResult = await CheckPreconditionsAsync(context, services).ConfigureAwait(false);
            if (!preconditionResult.IsSuccess)
                return await InvokeEventAndReturn(context, preconditionResult).ConfigureAwait(false);

            IResult argsResult = await ParseArgumentsAsync(context, services).ConfigureAwait(false);

            if (!argsResult.IsSuccess)
                return await InvokeEventAndReturn(context, argsResult).ConfigureAwait(false);

            if (argsResult is not ParseResult parseResult)
                return ExecuteResult.FromError(InteractionCommandError.BadArgs, "Complex command parsing failed for an unknown reason.");

            object?[] args = parseResult.Args ?? [];

            int index = 0;
            foreach (TParameter parameter in Parameters)
            {
                PreconditionResult result = await parameter.CheckPreconditionsAsync(context, args[index++], services).ConfigureAwait(false);
                if (!result.IsSuccess)
                    return await InvokeEventAndReturn(context, result).ConfigureAwait(false);
            }

            if (_action is not null)
            {
                Task task = _action(context, args, services, this);

                if (task is Task<IResult> resultTask)
                {
                    IResult result = await resultTask.ConfigureAwait(false);
                    await InvokeModuleEvent(context, result).ConfigureAwait(false);
                    if (result is RuntimeResult or ExecuteResult)
                        return result;
                }
                else
                {
                    await task.ConfigureAwait(false);
                    return await InvokeEventAndReturn(context, ExecuteResult.FromSuccess()).ConfigureAwait(false);
                }
            }

            return await InvokeEventAndReturn(context, ExecuteResult.FromError(InteractionCommandError.Unsuccessful, "Command execution failed for an unknown reason")).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Exception innerEx = ex;
            while (innerEx is TargetInvocationException { InnerException: { } nestedEx })
                innerEx = nestedEx;

            InteractionException interactionException = new(this, context, innerEx);
            await Module.CommandService._cmdLogger.ErrorAsync(interactionException).ConfigureAwait(false);

            ExecuteResult result = ExecuteResult.FromError(innerEx);
            await InvokeModuleEvent(context, result).ConfigureAwait(false);

            if (Module.CommandService._throwOnError)
            {
                if (innerEx == ex)
                    throw;
                else
                    ExceptionDispatchInfo.Capture(innerEx).Throw();
            }

            return result;
        }
        finally
        {
            await CommandService._cmdLogger.VerboseAsync($"Executed {GetLogString(context)}").ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     调用此命令的模块事件。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <param name="result"> 命令的执行结果。 </param>
    /// <returns> 表示异步调用操作的任务。 </returns>
    protected abstract Task InvokeModuleEvent(IInteractionContext context, IResult result);

    /// <summary>
    ///     获取此命令的日志字符串。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <returns> 此命令的日志字符串。 </returns>
    protected abstract string GetLogString(IInteractionContext context);

    /// <inheritdoc/>
    public async Task<PreconditionResult> CheckPreconditionsAsync(IInteractionContext context, IServiceProvider? services)
    {
        async Task<PreconditionResult> CheckGroups(ILookup<string?, PreconditionAttribute> preconditions, string type)
        {
            foreach (IGrouping<string?, PreconditionAttribute> preconditionGroup in preconditions)
            {
                if (preconditionGroup.Key == null)
                {
                    foreach (PreconditionAttribute precondition in preconditionGroup)
                    {
                        PreconditionResult result = await precondition.CheckRequirementsAsync(context, this, services).ConfigureAwait(false);
                        if (!result.IsSuccess)
                            return result;
                    }
                }
                else
                {
                    List<PreconditionResult> results = new List<PreconditionResult>();
                    foreach (PreconditionAttribute precondition in preconditionGroup)
                        results.Add(await precondition.CheckRequirementsAsync(context, this, services).ConfigureAwait(false));

                    if (!results.Any(p => p.IsSuccess))
                        return PreconditionGroupResult.FromError($"{type} precondition group {preconditionGroup.Key} failed.", results);
                }
            }
            return PreconditionGroupResult.FromSuccess();
        }

        PreconditionResult moduleResult = await CheckGroups(Module.GroupedPreconditions, "Module").ConfigureAwait(false);
        if (!moduleResult.IsSuccess)
            return moduleResult;

        PreconditionResult commandResult = await CheckGroups(_groupedPreconditions, "Command").ConfigureAwait(false);
        return !commandResult.IsSuccess ? commandResult : PreconditionResult.FromSuccess();
    }

    /// <summary>
    ///     调用此命令的模块事件并返回结果。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <param name="result"> 命令的执行结果。 </param>
    /// <typeparam name="T"> 结果的类型。 </typeparam>
    /// <returns> 命令的执行结果。 </returns>
    protected async Task<T> InvokeEventAndReturn<T>(IInteractionContext context, T result) where T : IResult
    {
        await InvokeModuleEvent(context, result).ConfigureAwait(false);
        return result;
    }

    // ICommandInfo

    /// <inheritdoc/>
    IReadOnlyCollection<IParameterInfo> ICommandInfo.Parameters => Parameters;

    /// <inheritdoc/>
    public override string ToString() => Name;
}
