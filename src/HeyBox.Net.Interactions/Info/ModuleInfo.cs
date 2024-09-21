using System.Collections.Immutable;
using HeyBox.Interactions.Builders;

namespace HeyBox.Interactions;

/// <summary>
///     Contains the information of a Interactions Module.
/// </summary>
public class ModuleInfo
{
    internal ILookup<string?, PreconditionAttribute> GroupedPreconditions { get; }

    /// <summary>
    ///     Gets the underlying command service.
    /// </summary>
    public InteractionService CommandService { get; }

    /// <summary>
    ///     Gets the name of this module class.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the description of this module.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     Gets the Slash Commands that are declared in this module.
    /// </summary>
    public IReadOnlyList<SlashCommandInfo> SlashCommands { get; }

    /// <summary>
    ///     Gets a collection of the attributes of this module.
    /// </summary>
    public IReadOnlyCollection<Attribute> Attributes { get; }

    /// <summary>
    ///     Gets a collection of the preconditions of this module.
    /// </summary>
    public IReadOnlyCollection<PreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     Gets the context types commands in this module can be executed in.
    /// </summary>
    public IReadOnlyCollection<InteractionContextType> ContextTypes { get; }

    internal ModuleInfo(ModuleBuilder builder, InteractionService commandService, IServiceProvider? services)
    {
        CommandService = commandService;

        Name = builder.Name ?? throw new InvalidOperationException("Module name must be set.");
        Description = builder.Description;
        SlashCommands = BuildSlashCommands(builder).ToImmutableArray();
        Attributes = BuildAttributes(builder).ToImmutableArray();
        Preconditions = BuildPreconditions(builder).ToImmutableArray();
        ContextTypes = builder.ContextTypes?.ToImmutableArray() ?? throw new InvalidOperationException("Context types must be set.");

        GroupedPreconditions = Preconditions.ToLookup(x => x.Group, x => x, StringComparer.Ordinal);
    }

    private IEnumerable<SlashCommandInfo> BuildSlashCommands(ModuleBuilder builder)
    {
        var result = new List<SlashCommandInfo>();

        foreach (Builders.SlashCommandBuilder commandBuilder in builder.SlashCommands)
            result.Add(commandBuilder.Build(this, CommandService));

        return result;
    }

    private IEnumerable<Attribute> BuildAttributes(ModuleBuilder builder)
    {
        return builder.Attributes;
    }

    private static IEnumerable<PreconditionAttribute> BuildPreconditions(ModuleBuilder builder)
    {
        return builder.Preconditions;
    }
}
