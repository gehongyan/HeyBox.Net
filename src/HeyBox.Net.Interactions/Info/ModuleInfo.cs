using System.Collections.Immutable;
using HeyBox.Interactions.Builders;

namespace HeyBox.Interactions;

/// <summary>
///     表示交互模块的信息。
/// </summary>
public class ModuleInfo
{
    internal ILookup<string?, PreconditionAttribute> GroupedPreconditions { get; }

    /// <summary>
    ///     获取底层交互服务。
    /// </summary>
    public InteractionService CommandService { get; }

    /// <summary>
    ///     获取此模块的名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取此模块的描述。
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     获取在此模块中声明的斜线命令。
    /// </summary>
    public IReadOnlyList<SlashCommandInfo> SlashCommands { get; }

    /// <summary>
    ///     获取此模块的属性集合。
    /// </summary>
    public IReadOnlyCollection<Attribute> Attributes { get; }

    /// <summary>
    ///     获取此模块的前置条件集合。
    /// </summary>
    public IReadOnlyCollection<PreconditionAttribute> Preconditions { get; }

    /// <summary>
    ///     获取可以在其中执行此模块命令的上下文类型。
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
