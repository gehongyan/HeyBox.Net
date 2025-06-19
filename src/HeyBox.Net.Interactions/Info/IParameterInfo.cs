namespace HeyBox.Interactions;

/// <summary>
///     表示 <see cref="ICommandInfo"/> 的参数。
/// </summary>
public interface IParameterInfo
{
    /// <summary>
    ///     获取此参数所属的命令。
    /// </summary>
    ICommandInfo Command { get; }

    /// <summary>
    ///     获取此参数的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此参数的类型。
    /// </summary>
    Type ParameterType { get; }

    /// <summary>
    ///     获取此参数是否为必填项。
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    ///     获取此参数是否被 <see langword="params"/> 关键字标记。
    /// </summary>
    bool IsParameterArray { get; }

    /// <summary>
    ///     获取此参数的默认值（如果为可选参数）。
    /// </summary>
    object? DefaultValue { get; }

    /// <summary>
    ///     获取此参数的特性集合。
    /// </summary>
    IReadOnlyCollection<Attribute> Attributes { get; }

    /// <summary>
    ///     获取此参数的先决条件集合。
    /// </summary>
    IReadOnlyCollection<ParameterPreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     检查执行上下文是否满足参数的先决条件要求。
    /// </summary>
    Task<PreconditionResult> CheckPreconditionsAsync(IInteractionContext context, object? value, IServiceProvider? services);
}
