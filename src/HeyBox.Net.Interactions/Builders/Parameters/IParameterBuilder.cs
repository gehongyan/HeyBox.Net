namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="IParameterInfo"/> 的命令参数生成器。
/// </summary>
public interface IParameterBuilder
{
    /// <summary>
    ///     获取此参数所属的父命令。
    /// </summary>
    ICommandBuilder Command { get; }

    /// <summary>
    ///     获取此参数的名称。
    /// </summary>
    string? Name { get; }

    /// <summary>
    ///     获取此参数的类型。
    /// </summary>
    Type? ParameterType { get; }

    /// <summary>
    ///     获取此参数是否为必填项。
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    ///     获取此参数是否为 <see langword="params"/>。
    /// </summary>
    bool IsParameterArray { get; }

    /// <summary>
    ///     获取此参数的默认值。
    /// </summary>
    object? DefaultValue { get; }

    /// <summary>
    ///     获取此命令的特性集合。
    /// </summary>
    IReadOnlyCollection<Attribute> Attributes { get; }

    /// <summary>
    ///     获取此命令的先决条件集合。
    /// </summary>
    IReadOnlyCollection<ParameterPreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     设置 <see cref="Name"/>。
    /// </summary>
    /// <param name="name"> <see cref="Name"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder WithName(string name);

    /// <summary>
    ///     设置 <see cref="ParameterType"/>。
    /// </summary>
    /// <param name="type"> <see cref="ParameterType"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder SetParameterType(Type type);

    /// <summary>
    ///     设置 <see cref="IsRequired"/>。
    /// </summary>
    /// <param name="isRequired"> <see cref="IsRequired"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder SetRequired(bool isRequired);

    /// <summary>
    ///     设置 <see cref="DefaultValue"/>。
    /// </summary>
    /// <param name="defaultValue"> <see cref="DefaultValue"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder SetDefaultValue(object defaultValue);

    /// <summary>
    ///     向 <see cref="Attributes"/> 添加特性。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Attributes"/> 的新特性。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder AddAttributes(params Attribute[] attributes);

    /// <summary>
    ///     向 <see cref="Preconditions"/> 添加先决条件。
    /// </summary>
    /// <param name="preconditions"> 要添加到 <see cref="Preconditions"/> 的新先决条件。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    IParameterBuilder AddPreconditions(params ParameterPreconditionAttribute[] preconditions);
}
