namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="CommandInfo{TParameter}"/> 的基础生成器类。
/// </summary>
/// <typeparam name="TInfo"> 构建后生成的 <see cref="CommandInfo{TParameter}"/> 类型。 </typeparam>
/// <typeparam name="TBuilder"> 继承的 <see cref="CommandBuilder{TInfo, TBuilder, TParamBuilder}"/> 类型。 </typeparam>
/// <typeparam name="TParamBuilder"> 此命令参数的生成器类型。 </typeparam>
public abstract class CommandBuilder<TInfo, TBuilder, TParamBuilder> : ICommandBuilder
    where TInfo : class, ICommandInfo
    where TBuilder : CommandBuilder<TInfo, TBuilder, TParamBuilder>
    where TParamBuilder : class, IParameterBuilder
{
    private readonly List<Attribute> _attributes;
    private readonly List<PreconditionAttribute> _preconditions;
    private readonly List<TParamBuilder> _parameters;

    /// <summary>
    ///     获取生成器实例。
    /// </summary>
    protected abstract TBuilder Instance { get; }

    /// <inheritdoc/>
    public ModuleBuilder Module { get; }

    /// <inheritdoc/>
    public ExecuteCallback? Callback { get; internal set; }

    /// <inheritdoc/>
    public string? Name { get; internal set; }

    /// <inheritdoc/>
    public string? MethodName { get; set; }

    /// <inheritdoc/>
    public bool TreatNameAsRegex { get; set; }

    /// <inheritdoc/>
    public RunMode RunMode { get; set; }

    /// <inheritdoc/>
    public IReadOnlyList<Attribute> Attributes => _attributes;

    /// <summary>
    ///     获取此命令的参数生成器列表。
    /// </summary>
    public IReadOnlyList<TParamBuilder> Parameters => _parameters;

    /// <inheritdoc/>
    public IReadOnlyList<PreconditionAttribute> Preconditions => _preconditions;

    /// <inheritdoc/>
    IReadOnlyList<IParameterBuilder> ICommandBuilder.Parameters => Parameters;

    internal CommandBuilder(ModuleBuilder module)
    {
        _attributes = [];
        _preconditions = [];
        _parameters = [];

        Module = module;
    }

    /// <summary>
    ///     使用提供的 <see cref="ModuleBuilder"/> 初始化一个新的 <see cref="CommandBuilder{TInfo, TBuilder, TParamBuilder}"/>。
    /// </summary>
    /// <param name="module"> 此命令所属的 <see cref="ModuleBuilder"/>。 </param>
    /// <param name="name"> 命令名称。 </param>
    /// <param name="callback"> 命令被触发时执行的回调。 </param>
    protected CommandBuilder(ModuleBuilder module, string name, ExecuteCallback callback) : this(module)
    {
        Name = name;
        Callback = callback;
    }

    /// <summary>
    ///     设置 <see cref="Name"/>。
    /// </summary>
    /// <param name="name"> <see cref="Name"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder WithName(string name)
    {
        Name = name;
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="MethodName"/>。
    /// </summary>
    /// <param name="name"> <see cref="MethodName"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder WithMethodName(string name)
    {
        MethodName = name;
        return Instance;
    }

    /// <summary>
    ///     向 <see cref="Attributes"/> 添加特性。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Attributes"/> 的新特性。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder WithAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="RunMode"/>。
    /// </summary>
    /// <param name="runMode"> <see cref="RunMode"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder SetRunMode(RunMode runMode)
    {
        RunMode = runMode;
        return Instance;
    }

    /// <summary>
    ///     设置 <see cref="TreatNameAsRegex"/>。
    /// </summary>
    /// <param name="value"> <see cref="TreatNameAsRegex"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder WithNameAsRegex(bool value)
    {
        TreatNameAsRegex = value;
        return Instance;
    }

    /// <summary>
    ///     向 <see cref="Parameters"/> 添加参数生成器。
    /// </summary>
    /// <param name="parameters"> 要添加到 <see cref="Parameters"/> 的新参数生成器。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder AddParameters(params TParamBuilder[] parameters)
    {
        _parameters.AddRange(parameters);
        return Instance;
    }

    /// <summary>
    ///     向 <see cref="Preconditions"/> 添加先决条件。
    /// </summary>
    /// <param name="preconditions"> 要添加到 <see cref="Preconditions"/> 的新先决条件。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public TBuilder WithPreconditions(params PreconditionAttribute[] preconditions)
    {
        _preconditions.AddRange(preconditions);
        return Instance;
    }

    /// <summary>
    ///     向命令添加参数。
    /// </summary>
    /// <param name="configure"> 配置参数的委托。 </param>
    /// <returns> 生成器实例。 </returns>
    public abstract TBuilder AddParameter(Action<TParamBuilder> configure);

    internal abstract TInfo Build(ModuleInfo module, InteractionService commandService);

    //ICommandBuilder
    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.WithName(string name) =>
        WithName(name);

    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.WithMethodName(string name) =>
        WithMethodName(name);
    ICommandBuilder ICommandBuilder.WithAttributes(params Attribute[] attributes) =>
        WithAttributes(attributes);

    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.SetRunMode(RunMode runMode) =>
        SetRunMode(runMode);

    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.WithNameAsRegex(bool value) =>
        WithNameAsRegex(value);

    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.AddParameters(params IParameterBuilder[] parameters) =>
        AddParameters([..parameters.Cast<TParamBuilder>()]);

    /// <inheritdoc/>
    ICommandBuilder ICommandBuilder.WithPreconditions(params PreconditionAttribute[] preconditions) =>
        WithPreconditions(preconditions);
}
