namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="ICommandInfo"/> 的命令生成器。
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    ///     获取此命令的执行委托。
    /// </summary>
    ExecuteCallback? Callback { get; }

    /// <summary>
    ///     获取此命令的父模块。
    /// </summary>
    ModuleBuilder Module { get; }

    /// <summary>
    ///     获取此命令的名称。
    /// </summary>
    string? Name { get; }

    /// <summary>
    ///     获取或设置此命令的方法名。
    /// </summary>
    string? MethodName { get; set; }

    /// <summary>
    ///     获取或设置 <see cref="Name"/> 是否应直接作为正则表达式模式使用。
    /// </summary>
    bool TreatNameAsRegex { get; set; }

    /// <summary>
    ///     获取或设置此命令的运行模式。
    /// </summary>
    RunMode RunMode { get; set; }

    /// <summary>
    ///     获取此命令的特性集合。
    /// </summary>
    IReadOnlyList<Attribute> Attributes { get; }

    /// <summary>
    ///     获取此命令的参数集合。
    /// </summary>
    IReadOnlyList<IParameterBuilder> Parameters { get; }

    /// <summary>
    ///     获取此命令的先决条件集合。
    /// </summary>
    IReadOnlyList<PreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     设置 <see cref="Name"/>。
    /// </summary>
    /// <param name="name"> <see cref="Name"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder WithName(string name);

    /// <summary>
    ///     设置 <see cref="MethodName"/>。
    /// </summary>
    /// <param name="name"> <see cref="MethodName"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder WithMethodName(string name);

    /// <summary>
    ///     向 <see cref="Attributes"/> 添加特性。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Attributes"/> 的新特性。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder WithAttributes(params Attribute[] attributes);

    /// <summary>
    ///     设置 <see cref="RunMode"/>。
    /// </summary>
    /// <param name="runMode"> <see cref="RunMode"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder SetRunMode(RunMode runMode);

    /// <summary>
    ///     设置 <see cref="TreatNameAsRegex"/>。
    /// </summary>
    /// <param name="value"> <see cref="TreatNameAsRegex"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder WithNameAsRegex(bool value);

    /// <summary>
    ///     向 <see cref="Parameters"/> 添加参数生成器。
    /// </summary>
    /// <param name="parameters"> 要添加到 <see cref="Parameters"/> 的新参数生成器。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder AddParameters(params IParameterBuilder[] parameters);

    /// <summary>
    ///     向 <see cref="Preconditions"/> 添加先决条件。
    /// </summary>
    /// <param name="preconditions"> 要添加到 <see cref="Preconditions"/> 的新先决条件。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    ICommandBuilder WithPreconditions(params PreconditionAttribute[] preconditions);
}
