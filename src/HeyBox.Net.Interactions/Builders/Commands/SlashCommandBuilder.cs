namespace HeyBox.Interactions.Builders;

/// <summary>
///     Represents a builder for creating <see cref="SlashCommandInfo"/>.
/// </summary>
public sealed class SlashCommandBuilder : CommandBuilder<SlashCommandInfo, SlashCommandBuilder, SlashCommandParameterBuilder>
{
    /// <inheritdoc />
    protected override SlashCommandBuilder Instance => this;

    /// <summary>
    ///     Gets and sets the description of this command.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the context types this command can be executed in.
    /// </summary>
    public HashSet<InteractionContextType>? ContextTypes { get; set; }

    internal SlashCommandBuilder(ModuleBuilder module) : base(module)
    {
        ContextTypes = module.ContextTypes;
    }

    /// <summary>
    ///     Initializes a new <see cref="SlashCommandBuilder"/>.
    /// </summary>
    /// <param name="module">Parent module of this command.</param>
    /// <param name="name">Name of this command.</param>
    /// <param name="callback">Execution callback of this command.</param>
    public SlashCommandBuilder(ModuleBuilder module, string name, ExecuteCallback callback)
        : base(module, name, callback) { }

    /// <summary>
    ///     Sets <see cref="Description"/>.
    /// </summary>
    /// <param name="description">New value of the <see cref="Description"/>.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public SlashCommandBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     Adds a command parameter to the parameters collection.
    /// </summary>
    /// <param name="configure"><see cref="SlashCommandParameterBuilder"/> factory.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public override SlashCommandBuilder AddParameter(Action<SlashCommandParameterBuilder> configure)
    {
        var parameter = new SlashCommandParameterBuilder(this);
        configure(parameter);
        AddParameters(parameter);
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="ContextTypes"/> on this <see cref="SlashCommandBuilder"/>.
    /// </summary>
    /// <param name="contextTypes">Context types the command can be executed in.</param>
    /// <returns>The builder instance.</returns>
    public SlashCommandBuilder WithContextTypes(params InteractionContextType[] contextTypes)
    {
        ContextTypes = [..contextTypes];
        return this;
    }

    internal override SlashCommandInfo Build(ModuleInfo module, InteractionService commandService) =>
        new SlashCommandInfo(this, module, commandService);
}
