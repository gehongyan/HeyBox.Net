namespace HeyBox.Interactions;

/// <summary>
///     表示一个通用的交互模块基类。
/// </summary>
public interface IInteractionModuleBase
{
    /// <summary>
    ///     设置此交互模块的上下文。
    /// </summary>
    /// <param name="context"> 交互上下文。 </param>
    void SetContext(IInteractionContext context);

    /// <summary>
    ///     在执行应用程序命令之前异步执行的方法。
    /// </summary>
    /// <param name="command"> 相关的命令信息。 </param>
    Task BeforeExecuteAsync(ICommandInfo command);

    /// <summary>
    ///     在执行应用程序命令之前同步执行的方法。
    /// </summary>
    /// <param name="command"> 相关的命令信息。 </param>
    void BeforeExecute(ICommandInfo command);

    /// <summary>
    ///     在应用程序命令执行后异步执行的方法。
    /// </summary>
    /// <param name="command"> 相关的命令信息。 </param>
    Task AfterExecuteAsync(ICommandInfo command);

    /// <summary>
    ///     在应用程序命令执行后同步执行的方法。
    /// </summary>
    /// <param name="command"> 相关的命令信息。 </param>
    void AfterExecute(ICommandInfo command);

    /// <summary>
    ///     当调用 <see cref="HeyBox.Interactions.Builders.ModuleBuilder.Build(HeyBox.Interactions.InteractionService,System.IServiceProvider)"/> 时执行的方法。
    /// </summary>
    /// <param name="commandService"> 构建此方法的命令服务实例。 </param>
    /// <param name="module"> 此模块的信息。 </param>
    void OnModuleBuilding(InteractionService commandService, ModuleInfo module);

    /// <summary>
    ///     当自动模块创建完成后并在调用 <see cref="Builders.ModuleBuilder.Build(InteractionService, System.IServiceProvider)"/> 之前执行的方法。
    /// </summary>
    /// <param name="builder"> 构建此方法的模块构建器。 </param>
    /// <param name="commandService"> 构建此方法的命令服务实例。 </param>
    void Construct(Builders.ModuleBuilder builder, InteractionService commandService);
}
