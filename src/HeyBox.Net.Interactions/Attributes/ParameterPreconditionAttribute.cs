namespace HeyBox.Interactions;

/// <summary>
///     要求参数在执行前满足指定的先决条件。
/// </summary>
/// <seealso cref="PreconditionAttribute"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
public abstract class ParameterPreconditionAttribute : Attribute
{
    /// <summary>
    ///     获取如果执行上下文未满足先决条件检查时返回的错误消息。
    /// </summary>
    /// <remarks>
    ///     在派生类中重写时，如果先决条件未满足，将使用提供的字符串作为错误消息。
    ///     对于未重写此属性的类，设置该属性无效。
    /// </remarks>
    public virtual string? ErrorMessage { get; }

    /// <summary>
    ///     检查在命令执行前是否满足条件。
    /// </summary>
    /// <param name="context"> 命令的上下文。 </param>
    /// <param name="parameterInfo"> 被检查的命令参数。 </param>
    /// <param name="value"> 参数的原始值。 </param>
    /// <param name="services"> 用于依赖注入的服务集合。 </param>
    public abstract Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        IParameterInfo parameterInfo, object? value, IServiceProvider? services);
}
