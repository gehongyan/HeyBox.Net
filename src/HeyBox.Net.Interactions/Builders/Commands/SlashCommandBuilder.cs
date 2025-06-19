namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="SlashCommandInfo"/> 的生成器。
/// </summary>
public sealed class SlashCommandBuilder : CommandBuilder<SlashCommandInfo, SlashCommandBuilder, SlashCommandParameterBuilder>
{
    /// <inheritdoc />
    protected override SlashCommandBuilder Instance => this;

    /// <summary>
    ///     获取或设置此命令的描述。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     获取或设置此命令可在哪些上下文类型中执行。
    /// </summary>
    public HashSet<InteractionContextType>? ContextTypes { get; set; }

    internal SlashCommandBuilder(ModuleBuilder module) : base(module)
    {
        ContextTypes = module.ContextTypes;
    }

    /// <summary>
    ///     初始化一个新的 <see cref="SlashCommandBuilder"/>。
    /// </summary>
    /// <param name="module"> 此命令的父模块。 </param>
    /// <param name="name"> 此命令的名称。 </param>
    /// <param name="callback"> 此命令的执行回调。 </param>
    public SlashCommandBuilder(ModuleBuilder module, string name, ExecuteCallback callback)
        : base(module, name, callback) { }

    /// <summary>
    ///     设置 <see cref="Description"/>。
    /// </summary>
    /// <param name="description"> <see cref="Description"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public SlashCommandBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     向参数集合添加命令参数。
    /// </summary>
    /// <param name="configure"> <see cref="SlashCommandParameterBuilder"/> 工厂方法。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public override SlashCommandBuilder AddParameter(Action<SlashCommandParameterBuilder> configure)
    {
        var parameter = new SlashCommandParameterBuilder(this);
        configure(parameter);
        AddParameters(parameter);
        return this;
    }

    /// <summary>
    ///     设置此 <see cref="SlashCommandBuilder"/> 的 <see cref="ContextTypes"/>。
    /// </summary>
    /// <param name="contextTypes"> 命令可执行的上下文类型。 </param>
    /// <returns>生成器实例。</returns>
    public SlashCommandBuilder WithContextTypes(params InteractionContextType[] contextTypes)
    {
        ContextTypes = [..contextTypes];
        return this;
    }

    internal override SlashCommandInfo Build(ModuleInfo module, InteractionService commandService) =>
        new SlashCommandInfo(this, module, commandService);
}
