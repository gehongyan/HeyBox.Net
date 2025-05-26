using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HeyBox.Interactions.Builders;
using HeyBox.Logging;
using HeyBox.WebSocket;

namespace HeyBox.Interactions;

/// <summary>
///     提供构建交互服务的框架。
/// </summary>
public class InteractionService : IDisposable
{
    /// <summary>
    ///     当生成一条日志消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.LogMessage"/> 参数是描述日志消息的结构。 </item>
    ///     </list>
    /// </remarks>
    public event Func<LogMessage, Task> Log
    {
        add => _logEvent.Add(value);
        remove => _logEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<LogMessage, Task>> _logEvent = new();

    /// <summary>
    ///     当交互执行时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.Interactions.ICommandInfo"/> 参数是交互的信息。 </item>
    ///     <item> <see cref="IInteractionContext"/> 参数是交互的上下文。 </item>
    ///     <item> <see cref="HeyBox.Interactions.IResult"/> 参数是交互的结果。 </item>
    ///     </list>
    /// </remarks>
    public event Func<ICommandInfo?, IInteractionContext, IResult, Task> InteractionExecuted
    {
        add => SlashCommandExecuted += value;
        remove => SlashCommandExecuted -= value;
    }

    /// <summary>
    ///     当斜线命令执行时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.Interactions.ICommandInfo"/> 参数是交互的信息。 </item>
    ///     <item> <see cref="IInteractionContext"/> 参数是交互的上下文。 </item>
    ///     <item> <see cref="HeyBox.Interactions.IResult"/> 参数是交互的结果。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SlashCommandInfo?, IInteractionContext, IResult, Task> SlashCommandExecuted {
        add => _slashCommandExecutedEvent.Add(value);
        remove => _slashCommandExecutedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SlashCommandInfo?, IInteractionContext, IResult, Task>> _slashCommandExecutedEvent = new();

    private readonly ConcurrentDictionary<Type, ModuleInfo> _typedModuleDefs;
    private readonly CommandMap<SlashCommandInfo> _slashCommandMap;
    private readonly HashSet<ModuleInfo> _moduleDefs;
    private readonly TypeMap<TypeConverter, ISlashCommandInteractionDataOption> _typeConverterMap;
    private readonly TypeMap<TypeReader, string> _typeReaderMap;
    private readonly SemaphoreSlim _lock;
    internal readonly Logger _cmdLogger;
    internal readonly LogManager _logManager;

    internal readonly bool _throwOnError, _useCompiledLambda, _autoServiceScopes;
    internal readonly string _wildCardExp;
    internal readonly RunMode _runMode;

    /// <summary>
    ///     获取所有加载到 <see cref="InteractionService"/> 中的模块。
    /// </summary>
    public IReadOnlyList<ModuleInfo> Modules => _moduleDefs.ToList();

    /// <summary>
    ///     获取所有加载到 <see cref="InteractionService"/> 中的斜线命令。
    /// </summary>
    public IReadOnlyList<SlashCommandInfo> SlashCommands => _moduleDefs.SelectMany(x => x.SlashCommands).ToList();

    /// <summary>
    ///     使用默认配置初始化一个 <see cref="InteractionService"/> 类的新实例。
    /// </summary>
    private InteractionService() : this(new InteractionServiceConfig()) { }

    /// <summary>
    ///     使用指定的配置初始化一个 <see cref="InteractionService"/> 类的新实例。
    /// </summary>
    /// <param name="config"> 交互服务的配置。 </param>
    public InteractionService(InteractionServiceConfig config)
    {
        _lock = new SemaphoreSlim(1, 1);
        _typedModuleDefs = new ConcurrentDictionary<Type, ModuleInfo>();
        _moduleDefs = [];

        _logManager = new LogManager(config.LogLevel);
        _logManager.Message += async msg => await _logEvent.InvokeAsync(msg).ConfigureAwait(false);
        _cmdLogger = _logManager.CreateLogger("App Commands");

        _slashCommandMap = new CommandMap<SlashCommandInfo>(this);

        _runMode = config.DefaultRunMode;
        if (_runMode == RunMode.Default)
            throw new InvalidOperationException($"RunMode cannot be set to {RunMode.Default}");


        _throwOnError = config.ThrowOnError;
        _wildCardExp = config.WildCardExpression;
        _useCompiledLambda = config.UseCompiledLambda;
        _autoServiceScopes = config.AutoServiceScopes;

        _typeConverterMap = new TypeMap<TypeConverter, ISlashCommandInteractionDataOption>(this, new ConcurrentDictionary<Type, TypeConverter>
        {
            [typeof(TimeSpan)] = new TimeSpanConverter()
        }, new ConcurrentDictionary<Type, Type>
        {
            [typeof(IChannel)] = typeof(DefaultChannelConverter<>),
            [typeof(IRole)] = typeof(DefaultRoleConverter<>),
            [typeof(IAttachment)] = typeof(DefaultAttachmentConverter<>),
            [typeof(IUser)] = typeof(DefaultUserConverter<>),
            [typeof(IConvertible)] = typeof(DefaultValueConverter<>),
            [typeof(Enum)] = typeof(EnumConverter<>),
            [typeof(Nullable<>)] = typeof(NullableConverter<>)
        });

        _typeReaderMap = new TypeMap<TypeReader, string>(this, new ConcurrentDictionary<Type, TypeReader>(),
            new ConcurrentDictionary<Type, Type>
            {
                [typeof(IChannel)] = typeof(DefaultChannelReader<>),
                [typeof(IRole)] = typeof(DefaultRoleReader<>),
                [typeof(IUser)] = typeof(DefaultUserReader<>),
                [typeof(IConvertible)] = typeof(DefaultValueReader<>),
                [typeof(Enum)] = typeof(EnumReader<>),
                [typeof(Nullable<>)] = typeof(NullableReader<>)
            });
    }

    /// <summary>
    ///     使用构建器工厂创建并加载 <see cref="ModuleInfo"/>。
    /// </summary>
    /// <param name="name"> 模块的名称。 </param>
    /// <param name="services"> 要使用的依赖注入服务提供程序，如果不使用依赖注入，则传入 <c>null</c>。 </param>
    /// <param name="buildFunc"> 构建模块的委托。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是所创建的模块信息。 </returns>
    public async Task<ModuleInfo> CreateModuleAsync(string name, IServiceProvider? services, Action<ModuleBuilder> buildFunc)
    {
        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            var builder = new ModuleBuilder(this, name);
            buildFunc(builder);

            var moduleInfo = builder.Build(this, services ?? EmptyServiceProvider.Instance);
            LoadModuleInternal(moduleInfo);

            return moduleInfo;
        }
        finally
        {
            _lock.Release();
        }
    }


    /// <summary>
    ///     从指定的程序集中发现并加载命令模块。
    /// </summary>
    /// <param name="assembly"> 定义命令模块的程序集。 </param>
    /// <param name="services"> 要使用的依赖注入服务提供程序，如果不使用依赖注入，则传入 <c>null</c>。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是所创建的所有模块信息。 </returns>
    public async Task<IEnumerable<ModuleInfo>> AddModulesAsync(Assembly assembly, IServiceProvider services)
    {
        await _lock.WaitAsync().ConfigureAwait(false);

        try
        {
            var types = await ModuleClassBuilder.SearchAsync(assembly, this);
            var moduleDefs = await ModuleClassBuilder.BuildAsync(types, this, services ?? EmptyServiceProvider.Instance);

            foreach (var info in moduleDefs)
            {
                _typedModuleDefs[info.Key] = info.Value;
                LoadModuleInternal(info.Value);
            }
            return moduleDefs.Values;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    ///     添加指定类型的模块。
    /// </summary>
    /// <typeparam name="T"> 要添加的模块的类型。 </typeparam>
    /// <param name="services"> 要使用的依赖注入服务提供程序，如果不使用依赖注入，则传入 <c>null</c>。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是所创建的模块信息。 </returns>
    /// <exception cref="ArgumentException"> 当模块已经被添加时引发。 </exception>
    /// <exception cref="InvalidOperationException"> 当类型 <typeparamref name="T"/> 不是有效的模块类型时引发。 </exception>
    public Task<ModuleInfo> AddModuleAsync<T>(IServiceProvider services) where T : class =>
        AddModuleAsync(typeof(T), services);

    /// <summary>
    ///     添加指定类型的模块。
    /// </summary>
    /// <param name="type"> 要添加的模块的类型。 </param>
    /// <param name="services"> 要使用的依赖注入服务提供程序，如果不使用依赖注入，则传入 <c>null</c>。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是所创建的模块信息。 </returns>
    /// <exception cref="ArgumentException"> 当模块已经被添加时引发。 </exception>
    /// <exception cref="InvalidOperationException"> 当类型 <paramref name="type"/> 不是有效的模块类型时引发。 </exception>
    public async Task<ModuleInfo> AddModuleAsync(Type type, IServiceProvider services)
    {
        if (!typeof(IInteractionModuleBase).IsAssignableFrom(type))
            throw new ArgumentException("Type parameter must be a type of Slash Module", nameof(type));

        await _lock.WaitAsync().ConfigureAwait(false);

        try
        {
            var typeInfo = type.GetTypeInfo();

            if (_typedModuleDefs.ContainsKey(typeInfo))
                throw new ArgumentException("Module definition for this type already exists.");

            var moduleDef = (await ModuleClassBuilder.BuildAsync(new List<TypeInfo> { typeInfo }, this, services ?? EmptyServiceProvider.Instance).ConfigureAwait(false)).FirstOrDefault();

            if (moduleDef.Value == default)
                throw new InvalidOperationException($"Could not build the module {typeInfo.FullName}, did you pass an invalid type?");

            if (!_typedModuleDefs.TryAdd(type, moduleDef.Value))
                throw new ArgumentException("Module definition for this type already exists.");

            _typedModuleDefs[moduleDef.Key] = moduleDef.Value;
            LoadModuleInternal(moduleDef.Value);

            return moduleDef.Value;
        }
        finally
        {
            _lock.Release();
        }
    }

    private void LoadModuleInternal(ModuleInfo module)
    {
        _moduleDefs.Add(module);

        foreach (var command in module.SlashCommands)
            _slashCommandMap.AddCommand(command);
    }

    /// <summary>
    ///     移除指定类型的模块。
    /// </summary>
    /// <typeparam name="T"> 要移除的模块的类型。 </typeparam>
    /// <returns> 一个表示异步操作的任务，任务的结果是是否成功移除模块。 </returns>
    public Task<bool> RemoveModuleAsync<T>() =>
        RemoveModuleAsync(typeof(T));

    /// <summary>
    ///     移除指定类型的模块。
    /// </summary>
    /// <param name="type"> 要移除的模块的类型。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是是否成功移除模块。 </returns>
    public async Task<bool> RemoveModuleAsync(Type type)
    {
        await _lock.WaitAsync().ConfigureAwait(false);

        try
        {
            if (!_typedModuleDefs.TryRemove(type, out var module))
                return false;

            return RemoveModuleInternal(module);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    ///     移除指定的模块。
    /// </summary>
    /// <param name="module"> 要移除的模块。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是是否成功移除模块。 </returns>
    public async Task<bool> RemoveModuleAsync(ModuleInfo module)
    {
        await _lock.WaitAsync().ConfigureAwait(false);

        try
        {
            var typeModulePair = _typedModuleDefs.FirstOrDefault(x => x.Value.Equals(module));

            if (!typeModulePair.Equals(default(KeyValuePair<Type, ModuleInfo>)))
                _typedModuleDefs.TryRemove(typeModulePair.Key, out var _);

            return RemoveModuleInternal(module);
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool RemoveModuleInternal(ModuleInfo moduleInfo)
    {
        if (!_moduleDefs.Remove(moduleInfo))
            return false;

        foreach (var command in moduleInfo.SlashCommands)
            _slashCommandMap.RemoveCommand(command);

        return true;
    }

    /// <summary>
    ///     使用 <see cref="ISlashCommandInteraction"/> 搜索已注册到当前服务的斜线命令。
    /// </summary>
    /// <param name="slashCommandInteraction"> 要进行搜索的斜线命令交互。 </param>
    /// <returns> 搜索结果。当成功时，结果包含找到的 <see cref="SlashCommandInfo"/>。 </returns>
    public SearchResult<SlashCommandInfo> SearchSlashCommand(ISlashCommandInteraction slashCommandInteraction)
        => _slashCommandMap.GetCommand(slashCommandInteraction.Data.GetCommandKeywords());

    /// <summary>
    ///     执行指定的交互。
    /// </summary>
    /// <param name="context"> 交互的上下文。 </param>
    /// <param name="services"> 要使用的依赖注入服务提供程序，如果不使用依赖注入，则传入 <c>null</c>。 </param>
    /// <returns> 一个表示异步操作的任务，任务的结果是交互的结果。 </returns>
    public async Task<IResult> ExecuteCommandAsync(IInteractionContext context, IServiceProvider? services)
    {
        var interaction = context.Interaction;

        return interaction switch
        {
            ISlashCommandInteraction slashCommand => await ExecuteSlashCommandAsync(context, slashCommand, services).ConfigureAwait(false),
            _ => throw new InvalidOperationException($"{interaction.Type} interaction type cannot be executed by the Interaction service"),
        };
    }

    private async Task<IResult> ExecuteSlashCommandAsync(IInteractionContext context, ISlashCommandInteraction interaction, IServiceProvider? services)
    {
        var keywords = interaction.Data.GetCommandKeywords();

        var result = _slashCommandMap.GetCommand(keywords);

        if (!result.IsSuccess)
        {
            await _cmdLogger.DebugAsync($"Unknown slash command, skipping execution ({string.Join(" ", keywords).ToUpper()})");

            await _slashCommandExecutedEvent.InvokeAsync(null, context, result).ConfigureAwait(false);
            return result;
        }
        return await result.Command.ExecuteAsync(context, services).ConfigureAwait(false);
    }

    private static void SetMatchesIfApplicable<T>(IInteractionContext context, SearchResult<T> searchResult)
        where T : class, ICommandInfo
    {
        if (context is not IRouteMatchContainer matchContainer)
            return;

        if (searchResult.RegexCaptureGroups?.Length > 0)
        {
            var matches = new RouteSegmentMatch[searchResult.RegexCaptureGroups.Length];
            for (var i = 0; i < searchResult.RegexCaptureGroups.Length; i++)
                matches[i] = new RouteSegmentMatch(searchResult.RegexCaptureGroups[i]);

            matchContainer.SetSegmentMatches(matches);
        }
        else
            matchContainer.SetSegmentMatches(Array.Empty<RouteSegmentMatch>());
    }

    internal TypeConverter GetTypeConverter(Type type, IServiceProvider? services = null)
        => _typeConverterMap.Get(type, services);

    /// <summary>
    ///     添加一个具体类型的类型转换器。
    /// </summary>
    /// <typeparam name="T"> 类型转换器的主要目标类型。 </typeparam>
    /// <param name="converter"> 类型转换器实例。 </param>
    public void AddTypeConverter<T>(TypeConverter converter) =>
        _typeConverterMap.AddConcrete<T>(converter);

    /// <summary>
    ///     添加一个具体类型的类型转换器。
    /// </summary>
    /// <param name="type"> 类型转换器的主要目标类型。 </param>
    /// <param name="converter"> 类型转换器实例。 </param>
    public void AddTypeConverter(Type type, TypeConverter converter) =>
        _typeConverterMap.AddConcrete(type, converter);

    /// <summary>
    ///     添加一个泛型类型的类型转换器。
    /// </summary>
    /// <typeparam name="T"> 类型转换器所转换类型的泛型约束。 </typeparam>
    /// <param name="converterType"> 泛型类型转换器实例。 </param>
    public void AddGenericTypeConverter<T>(Type converterType) =>
        _typeConverterMap.AddGeneric<T>(converterType);

    /// <summary>
    ///     添加一个泛型类型的类型转换器。
    /// </summary>
    /// <param name="targetType"> 类型转换器所转换类型的泛型约束。 </param>
    /// <param name="converterType"> 泛型类型转换器实例。 </param>
    public void AddGenericTypeConverter(Type targetType, Type converterType) =>
        _typeConverterMap.AddGeneric(targetType, converterType);

    internal TypeReader GetTypeReader(Type type, IServiceProvider? services = null) =>
        _typeReaderMap.Get(type, services);

    /// <summary>
    ///     添加一个具体类型的类型读取器。
    /// </summary>
    /// <typeparam name="T"> 类型读取器的主要目标类型。 </typeparam>
    /// <param name="reader"> 类型读取器实例。 </param>
    public void AddTypeReader<T>(TypeReader reader) =>
        AddTypeReader(typeof(T), reader);

    /// <summary>
    ///     添加一个具体类型的类型读取器。
    /// </summary>
    /// <param name="type"> 类型读取器的主要目标类型。 </param>
    /// <param name="reader"> 类型读取器实例。 </param>
    public void AddTypeReader(Type type, TypeReader reader) =>
        _typeReaderMap.AddConcrete(type, reader);

    /// <summary>
    ///     添加一个泛型类型的类型读取器。
    /// </summary>
    /// <typeparam name="T"> 类型读取器所读取类型的泛型约束。 </typeparam>
    /// <param name="readerType"> 泛型类型读取器实例。 </param>
    public void AddGenericTypeReader<T>(Type readerType) =>
        AddGenericTypeReader(typeof(T), readerType);

    /// <summary>
    ///     添加一个泛型类型的类型读取器。
    /// </summary>
    /// <param name="targetType"> 类型读取器所读取类型的泛型约束。 </param>
    /// <param name="readerType"> 泛型类型读取器实例。 </param>
    public void AddGenericTypeReader(Type targetType, Type readerType) =>
        _typeReaderMap.AddGeneric(targetType, readerType);

    /// <summary>
    ///     尝试移除一个具体类型的类型转换器。
    /// </summary>
    /// <remarks>
    ///     从当前交互服务中移除的类型读取器不会立即从加载的模块或命令实例中取消引用，您需要重新加载模块以使更改生效。
    /// </remarks>
    /// <typeparam name="T"> 类型转换器的主要目标类型。 </typeparam>
    /// <param name="reader"> 被移除的类型转换器实例。 </param>
    /// <returns> 移除操作是否成功。 </returns>
    public bool TryRemoveTypeReader<T>([NotNullWhen(true)] out TypeReader? reader)
        => TryRemoveTypeReader(typeof(T), out reader);

    /// <summary>
    ///     尝试移除一个具体类型的类型转换器。
    /// </summary>
    /// <remarks>
    ///     从当前交互服务中移除的类型读取器不会立即从加载的模块或命令实例中取消引用，您需要重新加载模块以使更改生效。
    /// </remarks>
    /// <param name="type"> 类型转换器的主要目标类型。 </param>
    /// <param name="reader"> 被移除的类型转换器实例。 </param>
    /// <returns> 移除操作是否成功。 </returns>
    public bool TryRemoveTypeReader(Type type, [NotNullWhen(true)] out TypeReader? reader)
        => _typeReaderMap.TryRemoveConcrete(type, out reader);

    /// <summary>
    ///     尝试移除一个泛型类型的类型转换器。
    /// </summary>
    /// <remarks>
    ///     从当前交互服务中移除的类型读取器不会立即从加载的模块或命令实例中取消引用，您需要重新加载模块以使更改生效。
    /// </remarks>
    /// <typeparam name="T"> 类型转换器所转换类型的泛型约束。 </typeparam>
    /// <param name="readerType"> 被移除的泛型类型转换器实例。 </param>
    /// <returns> 移除操作是否成功。 </returns>
    public bool TryRemoveGenericTypeReader<T>([NotNullWhen(true)] out Type? readerType)
        => TryRemoveGenericTypeReader(typeof(T), out readerType);

    /// <summary>
    ///     尝试移除一个泛型类型的类型转换器。
    /// </summary>
    /// <remarks>
    ///     从当前交互服务中移除的类型读取器不会立即从加载的模块或命令实例中取消引用，您需要重新加载模块以使更改生效。
    /// </remarks>
    /// <param name="type"> 类型转换器所转换类型的泛型约束。 </param>
    /// <param name="readerType"> 被移除的泛型类型转换器实例。 </param>
    /// <returns> 移除操作是否成功。 </returns>
    public bool TryRemoveGenericTypeReader(Type type, [NotNullWhen(true)] out Type? readerType)
        => _typeReaderMap.TryRemoveGeneric(type, out readerType);

    /// <summary>
    ///     获取斜线命令的信息。
    /// </summary>
    /// <typeparam name="TModule"> 声明斜线命令的模块类型，必需是 <see cref="InteractionModuleBase{T}"/> 的实现。 </typeparam>
    /// <param name="methodName"> 斜线命令处理方法的名称，推荐使用 <c>nameof</c> 引用。 </param>
    /// <returns> 斜线命令的信息。 </returns>
    /// <exception cref="InvalidOperationException"> 当未找到指定的斜线命令时引发。 </exception>
    public SlashCommandInfo GetSlashCommandInfo<TModule>(string methodName) where TModule : class
    {
        var module = GetModuleInfo<TModule>();
        return module.SlashCommands.First(x => x.MethodName == methodName);
    }

    /// <summary>
    ///     获取模块的信息。
    /// </summary>
    /// <typeparam name="TModule"> 模块的类型，必需是 <see cref="InteractionModuleBase{T}"/> 的实现。 </typeparam>
    /// <returns> 模块的信息。 </returns>
    public ModuleInfo GetModuleInfo<TModule>() where TModule : class
    {
        if (!typeof(IInteractionModuleBase).IsAssignableFrom(typeof(TModule)))
            throw new ArgumentException("Type parameter must be a type of Slash Module", nameof(TModule));
        var module = _typedModuleDefs[typeof(TModule)];
        return module ?? throw new InvalidOperationException($"{typeof(TModule).FullName} is not loaded to the Slash Command Service");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _lock.Dispose();
    }
}
