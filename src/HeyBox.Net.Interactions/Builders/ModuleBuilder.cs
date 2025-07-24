using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="ModuleInfo"/> 的生成器。
/// </summary>
public class ModuleBuilder
{
    private readonly List<Attribute> _attributes;
    private readonly List<PreconditionAttribute> _preconditions;
    private readonly List<SlashCommandBuilder> _slashCommands;

    /// <summary>
    ///     获取底层交互服务。
    /// </summary>
    public InteractionService InteractionService { get; }

    /// <summary>
    ///     获取此模块的名称。
    /// </summary>
    public string? Name { get; internal set; }

    /// <summary>
    ///     获取或设置此模块的描述。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     获取此模块的特性集合。
    /// </summary>
    public IReadOnlyList<Attribute> Attributes => _attributes;

    /// <summary>
    ///     获取此模块的先决条件集合。
    /// </summary>
    public IReadOnlyCollection<PreconditionAttribute> Preconditions => _preconditions;

    /// <summary>
    ///     获取此模块的斜线命令集合。
    /// </summary>
    public IReadOnlyList<SlashCommandBuilder> SlashCommands => _slashCommands;

    /// <summary>
    ///     获取或设置此命令可在哪些上下文类型中执行。
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
    ///     初始化一个新的 <see cref="ModuleBuilder"/>。
    /// </summary>
    /// <param name="interactionService"> 底层交互服务。 </param>
    /// <param name="name"> 此模块的名称。 </param>
    public ModuleBuilder(InteractionService interactionService, string name)
        : this(interactionService)
    {
        Name = name;
    }

    /// <summary>
    ///     设置 <see cref="Description"/>。
    /// </summary>
    /// <param name="description"> <see cref="Description"/> 的新值。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public ModuleBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     向 <see cref="Attributes"/> 添加特性。
    /// </summary>
    /// <param name="attributes"> 要添加到 <see cref="Attributes"/> 的新特性。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public ModuleBuilder AddAttributes(params Attribute[] attributes)
    {
        _attributes.AddRange(attributes);
        return this;
    }

    /// <summary>
    ///     向 <see cref="Preconditions"/> 添加先决条件。
    /// </summary>
    /// <param name="preconditions"> 要添加到 <see cref="Preconditions"/> 的新先决条件。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public ModuleBuilder AddPreconditions(params PreconditionAttribute[] preconditions)
    {
        _preconditions.AddRange(preconditions);
        return this;
    }

    /// <summary>
    ///     向 <see cref="SlashCommands"/> 添加斜线命令生成器。
    /// </summary>
    /// <param name="configure"> <see cref="SlashCommandBuilder"/> 工厂方法。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public ModuleBuilder AddSlashCommand(Action<SlashCommandBuilder> configure)
    {
        var command = new SlashCommandBuilder(this);
        configure(command);
        _slashCommands.Add(command);
        return this;
    }

    /// <summary>
    ///     向 <see cref="SlashCommands"/> 添加斜线命令生成器。
    /// </summary>
    /// <param name="name"> 命令名称。 </param>
    /// <param name="callback"> 要执行的命令回调。 </param>
    /// <param name="configure"> <see cref="SlashCommandBuilder"/> 工厂方法。 </param>
    /// <returns>
    ///     生成器实例。
    /// </returns>
    public ModuleBuilder AddSlashCommand(string name, ExecuteCallback callback, Action<SlashCommandBuilder> configure)
    {
        var command = new SlashCommandBuilder(this, name, callback);
        configure(command);
        _slashCommands.Add(command);
        return this;
    }

    /// <summary>
    ///     设置此 <see cref="ModuleBuilder"/> 的 <see cref="ContextTypes"/>。
    /// </summary>
    /// <param name="contextTypes"> 命令可执行的上下文类型。 </param>
    /// <returns>生成器实例。</returns>
    public ModuleBuilder WithContextTypes(params InteractionContextType[] contextTypes)
    {
        ContextTypes = [..contextTypes];
        return this;
    }

    internal ModuleInfo Build(InteractionService interactionService, IServiceProvider services)
    {
        if (TypeInfo is not null && ModuleClassBuilder.IsValidModuleDefinition(TypeInfo))
        {
            using IServiceScope? scope = services.CreateScope();
            if (interactionService._autoServiceScopes)
                services = scope?.ServiceProvider ?? EmptyServiceProvider.Instance;

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
