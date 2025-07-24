namespace HeyBox.Interactions;

/// <summary>
///     要求模块或类在执行前满足指定的先决条件。
/// </summary>
/// <seealso cref="ParameterPreconditionAttribute"/>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class PreconditionAttribute : Attribute
{
    /// <summary>
    ///     获取此先决条件所属的分组。
    /// </summary>
    /// <remarks>
    ///     <see cref="Preconditions" /> 中同一分组的先决条件只需满足其中一个即可视为成功（A || B）。
    ///     未指定 <see cref="Group" /> 或为 <see langword="null" /> 时，需全部满足（A &amp;&amp; B）。
    /// </remarks>
    public string? Group { get; set; }

    /// <summary>
    ///     获取如果执行上下文未通过先决条件检查时返回的错误消息。
    /// </summary>
    /// <remarks>
    ///     在派生类中重写时，如果先决条件未通过，将使用提供的字符串作为错误消息。
    ///     对于未重写此属性的类，设置该属性无效。
    /// </remarks>
    public virtual string? ErrorMessage { get; }

    /// <summary>
    ///     检查 <paramref name="commandInfo"/> 所代表的命令是否满足先决条件。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <param name="commandInfo"> 被执行的命令。 </param>
    /// <param name="services"> 用于依赖注入的服务集合。 </param>
    public abstract Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        ICommandInfo commandInfo, IServiceProvider? services);
}
