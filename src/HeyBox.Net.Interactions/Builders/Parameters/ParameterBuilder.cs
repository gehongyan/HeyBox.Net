namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="IParameterInfo"/> 的基础生成器类。
/// </summary>
/// <typeparam name="TInfo"> 构建后生成的 <see cref="IParameterInfo"/> 类型。 </typeparam>
/// <typeparam name="TBuilder"> 继承的 <see cref="ParameterBuilder{TInfo, TBuilder}"/> 类型。 </typeparam>
public abstract class ParameterBuilder<TInfo, TBuilder> : IParameterBuilder
    where TInfo : class, IParameterInfo
    where TBuilder : ParameterBuilder<TInfo, TBuilder>
{
    private readonly List<ParameterPreconditionAttribute> _preconditions;
    private readonly List<Attribute> _attributes;

    /// <inheritdoc/>
    public ICommandBuilder Command { get; }

    /// <inheritdoc/>
    public string? Name { get; internal set; }

    /// <inheritdoc/>
    public Type? ParameterType { get; private set; }

    /// <inheritdoc/>
    public bool IsRequired { get; set; } = true;

    /// <inheritdoc/>
    public bool IsParameterArray { get; set; } = false;

    /// <inheritdoc/>
    public object? DefaultValue { get; set; }

    /// <inheritdoc/>
    public IReadOnlyCollection<Attribute> Attributes => _attributes;

    /// <inheritdoc/>
    public IReadOnlyCollection<ParameterPreconditionAttribute> Preconditions => _preconditions;

    /// <summary>
    ///     获取生成器实例。
    /// </summary>
    protected abstract TBuilder Instance { get; }

    internal ParameterBuilder(ICommandBuilder command)
    {
        _attributes = new List<Attribute>();
        _preconditions = new List<ParameterPreconditionAttribute>();

        Command = command;
    }

    /// <summary>
    ///     初始化一个新的 <see cref="ParameterBuilder{TInfo, TBuilder}"/>。
    /// </summary>
    /// <param name="command"> 此参数所属的命令。 </param>
    /// <param name="name"> 参数名称。 </param>
    /// <param name="type"> 参数类型。 </param>
    protected ParameterBuilder(ICommandBuilder command, string name, Type type) : this(command)
    {
        Name = name;
        SetParameterType(type);
    }

    /// <summary>
    ///     设置 <see cref="Name"/>。
    /// </summary>
    /// <param name="name"> <see cref="Name"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder WithName(string name)
    {
        Name = name;
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="ParameterType"/>。
    /// </summary>
    /// <param name="type"> <see cref="ParameterType"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder SetParameterType(Type type)
    {
        ParameterType = type;
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="IsRequired"/>。
    /// </summary>
    /// <param name="isRequired"> <see cref="IsRequired"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder SetRequired(bool isRequired)
    {
        IsRequired = isRequired;
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="DefaultValue"/>。
    /// </summary>
    /// <param name="defaultValue"> <see cref="DefaultValue"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder SetDefaultValue(object defaultValue)
    {
        DefaultValue = defaultValue;
        return Instance;
    }

    /// <summary>
    ///     向 <see cref="Attributes"/> 添加特性。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Attributes"/> 的新特性。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder AddAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return Instance;
    }

    /// <summary>
    ///     向 <see cref="Preconditions"/> 添加先决条件。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Preconditions"/> 的新先决条件。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public virtual TBuilder AddPreconditions(params ParameterPreconditionAttribute[] attributes)
    {
        _preconditions.AddRange(attributes);
        return Instance;
    }

    internal abstract TInfo Build(ICommandInfo command);

    //IParameterBuilder
    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.WithName(string name) =>
        WithName(name);

    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.SetParameterType(Type type) =>
        SetParameterType(type);

    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.SetRequired(bool isRequired) =>
        SetRequired(isRequired);

    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.SetDefaultValue(object defaultValue) =>
        SetDefaultValue(defaultValue);

    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.AddAttributes(params Attribute[] attributes) =>
        AddAttributes(attributes);

    /// <inheritdoc/>
    IParameterBuilder IParameterBuilder.AddPreconditions(params ParameterPreconditionAttribute[] preconditions) =>
        AddPreconditions(preconditions);
}
