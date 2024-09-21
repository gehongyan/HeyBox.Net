using System.Reflection;

namespace HeyBox.Interactions.Builders;

/// <summary>
///     Represents a builder for creating <see cref="ModuleInfo"/>.
/// </summary>
public class ModuleBuilder
{
    private readonly List<Attribute> _attributes;
    private readonly List<PreconditionAttribute> _preconditions;
    private readonly List<SlashCommandBuilder> _slashCommands;

    /// <summary>
    ///     Gets the underlying Interaction Service.
    /// </summary>
    public InteractionService InteractionService { get; }

    /// <summary>
    ///     Gets the name of this module.
    /// </summary>
    public string? Name { get; internal set; }

    /// <summary>
    ///     Gets and sets the description of this module.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets a collection of the attributes of this module.
    /// </summary>
    public IReadOnlyList<Attribute> Attributes => _attributes;

    /// <summary>
    ///     Gets a collection of the preconditions of this module.
    /// </summary>
    public IReadOnlyCollection<PreconditionAttribute> Preconditions => _preconditions;

    /// <summary>
    ///     Gets a collection of the Slash Commands of this module.
    /// </summary>
    public IReadOnlyList<SlashCommandBuilder> SlashCommands => _slashCommands;

    /// <summary>
    ///     Gets or sets the context types this command can be executed in.
    /// </summary>
    public HashSet<InteractionContextType> ContextTypes { get; set; }

    internal TypeInfo? TypeInfo { get; set; }

    internal ModuleBuilder(InteractionService interactionService)
    {
        InteractionService = interactionService;

        _attributes = [];
        _slashCommands = [];
        _preconditions = [];
        ContextTypes = [];
    }

    /// <summary>
    ///     Initializes a new <see cref="ModuleBuilder"/>.
    /// </summary>
    /// <param name="interactionService">The underlying Interaction Service.</param>
    /// <param name="name">Name of this module.</param>
    public ModuleBuilder(InteractionService interactionService, string name)
        : this(interactionService)
    {
        Name = name;
    }

    /// <summary>
    ///     Sets <see cref="Description"/>.
    /// </summary>
    /// <param name="description">New value of the <see cref="Description"/>.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public ModuleBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     Adds attributes to <see cref="Attributes"/>.
    /// </summary>
    /// <param name="attributes">New attributes to be added to <see cref="Attributes"/>.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public ModuleBuilder AddAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    /// <summary>
    ///     Adds preconditions to <see cref="Preconditions"/>.
    /// </summary>
    /// <param name="preconditions">New preconditions to be added to <see cref="Preconditions"/>.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public ModuleBuilder AddPreconditions(params PreconditionAttribute[] preconditions)
    {
        _preconditions.AddRange(preconditions);
        return this;
    }

    /// <summary>
    ///     Adds slash command builder to <see cref="SlashCommands"/>.
    /// </summary>
    /// <param name="configure"><see cref="SlashCommandBuilder"/> factory.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public ModuleBuilder AddSlashCommand(Action<SlashCommandBuilder> configure)
    {
        var command = new SlashCommandBuilder(this);
        configure(command);
        _slashCommands.Add(command);
        return this;
    }

    /// <summary>
    ///     Adds slash command builder to <see cref="SlashCommands"/>.
    /// </summary>
    /// <param name="name">Name of the command.</param>
    /// <param name="callback">Command callback to be executed.</param>
    /// <param name="configure"><see cref="SlashCommandBuilder"/> factory.</param>
    /// <returns>
    ///     The builder instance.
    /// </returns>
    public ModuleBuilder AddSlashCommand(string name, ExecuteCallback callback, Action<SlashCommandBuilder> configure)
    {
        var command = new SlashCommandBuilder(this, name, callback);
        configure(command);
        _slashCommands.Add(command);
        return this;
    }

    /// <summary>
    ///     Sets  the <see cref="ContextTypes"/> on this <see cref="ModuleBuilder"/>.
    /// </summary>
    /// <param name="contextTypes">Context types the command can be executed in.</param>
    /// <returns>The builder instance.</returns>
    public ModuleBuilder WithContextTypes(params InteractionContextType[] contextTypes)
    {
        ContextTypes = [..contextTypes];
        return this;
    }

    internal ModuleInfo Build(InteractionService interactionService, IServiceProvider services)
    {
        if (TypeInfo is not null && ModuleClassBuilder.IsValidModuleDefinition(TypeInfo))
        {
            IInteractionModuleBase instance = ReflectionUtils<IInteractionModuleBase>.CreateObject(TypeInfo, interactionService, services);

            try
            {
                instance.Construct(this, interactionService);
                ModuleInfo moduleInfo = new(this, interactionService, services);
                instance.OnModuleBuilding(interactionService, moduleInfo);
                return moduleInfo;
            }
            finally
            {
                (instance as IDisposable)?.Dispose();
            }
        }

        return new ModuleInfo(this, interactionService, services);
    }
}
