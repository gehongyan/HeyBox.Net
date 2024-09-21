using System.Reflection;
using System.Text.RegularExpressions;

namespace HeyBox.Interactions.Builders;

internal static class ModuleClassBuilder
{
    private static readonly TypeInfo ModuleTypeInfo = typeof(IInteractionModuleBase).GetTypeInfo();

    public const int MaxCommandDepth = 3;

    public static async Task<IEnumerable<TypeInfo>> SearchAsync(Assembly assembly, InteractionService commandService)
    {
        static bool IsLoadableModule(TypeInfo info)
        {
            return info.DeclaredMethods.Any(x => x.GetCustomAttribute<SlashCommandAttribute>() != null);
        }

        List<TypeInfo> result = [];

        foreach (TypeInfo type in assembly.DefinedTypes)
        {
            if ((type.IsPublic || type.IsNestedPublic) && IsValidModuleDefinition(type))
            {
                result.Add(type);
            }
            else if (IsLoadableModule(type))
            {
                await commandService._cmdLogger.WarningAsync($"Class {type.FullName} is not public and cannot be loaded.").ConfigureAwait(false);
            }
        }
        return result;
    }

    public static async Task<Dictionary<Type, ModuleInfo>> BuildAsync(IEnumerable<TypeInfo> validTypes, InteractionService commandService,
        IServiceProvider services)
    {
        IEnumerable<TypeInfo> topLevelGroups = validTypes.Where(x => x.DeclaringType == null || !IsValidModuleDefinition(x.DeclaringType.GetTypeInfo()));
        List<TypeInfo> built = [];

        Dictionary<Type, ModuleInfo> result = new();

        foreach (TypeInfo type in topLevelGroups)
        {
            ModuleBuilder builder = new(commandService);

            BuildModule(builder, type, commandService, services);
            built.Add(type);

            ModuleInfo moduleInfo = builder.Build(commandService, services);

            result.Add(type.AsType(), moduleInfo);
        }

        await commandService._cmdLogger.DebugAsync($"Successfully built {built.Count} Slash Command modules.").ConfigureAwait(false);

        return result;
    }

    private static void BuildModule(ModuleBuilder builder, TypeInfo typeInfo, InteractionService commandService,
        IServiceProvider services)
    {
        IEnumerable<Attribute> attributes = typeInfo.GetCustomAttributes();

        builder.Name = typeInfo.Name;
        builder.TypeInfo = typeInfo;

        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case PreconditionAttribute precondition:
                    builder.AddPreconditions(precondition);
                    break;
                case CommandContextTypeAttribute contextType:
                    builder.WithContextTypes([..contextType.ContextTypes]);
                    break;
                default:
                    builder.AddAttributes(attribute);
                    break;
            }
        }

        MethodInfo[] methods = typeInfo.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        IEnumerable<MethodInfo> validSlashCommands = methods.Where(IsValidSlashCommandDefinition);

        Func<IServiceProvider?, IInteractionModuleBase> createInstance = commandService._useCompiledLambda
            ? ReflectionUtils<IInteractionModuleBase>.CreateLambdaBuilder(typeInfo, commandService)
            : ReflectionUtils<IInteractionModuleBase>.CreateBuilder(typeInfo, commandService);

        foreach (MethodInfo method in validSlashCommands)
            builder.AddSlashCommand(x => BuildSlashCommand(x, createInstance, method, commandService, services));
    }

    private static void BuildSlashCommand(SlashCommandBuilder builder, Func<IServiceProvider?, IInteractionModuleBase> createInstance, MethodInfo methodInfo,
        InteractionService commandService, IServiceProvider services)
    {
        IEnumerable<Attribute> attributes = methodInfo.GetCustomAttributes();

        builder.MethodName = methodInfo.Name;

        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case SlashCommandAttribute command:
                {
                    builder.Name = command.Name;
                    builder.Description = command.Description;
                    builder.RunMode = command.RunMode;
                }
                    break;
                case PreconditionAttribute precondition:
                    builder.WithPreconditions(precondition);
                    break;
                case CommandContextTypeAttribute contextType:
                    builder.WithContextTypes(contextType.ContextTypes.ToArray());
                    break;
                default:
                    builder.WithAttributes(attribute);
                    break;
            }
        }

        ParameterInfo[] parameters = methodInfo.GetParameters();

        foreach (ParameterInfo parameter in parameters)
            builder.AddParameter(x => BuildSlashParameter(x, parameter, services));

        builder.Callback = CreateCallback(createInstance, methodInfo, commandService);
    }

    private static ExecuteCallback CreateCallback(Func<IServiceProvider?, IInteractionModuleBase> createInstance,
        MethodInfo methodInfo, InteractionService commandService)
    {
        Func<IInteractionModuleBase, object?[], Task> commandInvoker = commandService._useCompiledLambda
            ? ReflectionUtils<IInteractionModuleBase>.CreateMethodInvoker(methodInfo)
            : (module, args) => methodInfo.Invoke(module, args) as Task ?? Task.CompletedTask;

        async Task<IResult> ExecuteCallback(IInteractionContext context, object?[] args, IServiceProvider? serviceProvider, ICommandInfo commandInfo)
        {
            IInteractionModuleBase instance = createInstance(serviceProvider);
            instance.SetContext(context);

            try
            {
                await instance.BeforeExecuteAsync(commandInfo).ConfigureAwait(false);
                instance.BeforeExecute(commandInfo);
                Task task = commandInvoker(instance, args) ?? Task.Delay(0);

                if (task is Task<RuntimeResult> runtimeTask)
                    return await runtimeTask.ConfigureAwait(false);
                else
                {
                    await task.ConfigureAwait(false);
                    return ExecuteResult.FromSuccess();
                }
            }
            catch (Exception ex)
            {
                InteractionException interactionException = new(commandInfo, context, ex);
                await commandService._cmdLogger.ErrorAsync(interactionException).ConfigureAwait(false);
                return ExecuteResult.FromError(interactionException);
            }
            finally
            {
                await instance.AfterExecuteAsync(commandInfo).ConfigureAwait(false);
                instance.AfterExecute(commandInfo);
                (instance as IDisposable)?.Dispose();
            }
        }

        return ExecuteCallback;
    }

    #region Parameters
    private static void BuildSlashParameter(SlashCommandParameterBuilder builder, ParameterInfo paramInfo, IServiceProvider services)
    {
        IEnumerable<Attribute> attributes = paramInfo.GetCustomAttributes();
        Type paramType = paramInfo.ParameterType;

        builder.Name = paramInfo.Name;
        builder.Description = paramInfo.Name;
        builder.IsRequired = !paramInfo.IsOptional;
        builder.DefaultValue = paramInfo.DefaultValue;

        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case SummaryAttribute description:
                {
                    if (!string.IsNullOrEmpty(description.Name))
                        builder.Name = description.Name;

                    if (!string.IsNullOrEmpty(description.Description))
                        builder.Description = description.Description;
                }
                    break;
                case ChoiceAttribute choice:
                    builder.WithChoices(new ParameterChoice(choice.Name, choice.Value));
                    break;
                case ParamArrayAttribute _:
                    builder.IsParameterArray = true;
                    break;
                case ParameterPreconditionAttribute precondition:
                    builder.AddPreconditions(precondition);
                    break;
                case ChannelTypesAttribute channelTypes:
                    builder.WithChannelTypes(channelTypes.ChannelTypes);
                    break;
                case MaxValueAttribute maxValue:
                    builder.MaxValue = maxValue.Value;
                    break;
                case MinValueAttribute minValue:
                    builder.MinValue = minValue.Value;
                    break;
                case MinLengthAttribute minLength:
                    builder.MinLength = minLength.Length;
                    break;
                case MaxLengthAttribute maxLength:
                    builder.MaxLength = maxLength.Length;
                    break;
                case ComplexParameterAttribute complexParameter:
                {
                    builder.IsComplexParameter = true;
                    ConstructorInfo ctor = GetComplexParameterConstructor(paramInfo.ParameterType.GetTypeInfo(), complexParameter);

                    foreach (ParameterInfo parameter in ctor.GetParameters())
                    {
                        if (parameter.IsDefined(typeof(ComplexParameterAttribute)))
                            throw new InvalidOperationException("You cannot create nested complex parameters.");

                        builder.AddComplexParameterField(fieldBuilder => BuildSlashParameter(fieldBuilder, parameter, services));
                    }

                    Func<object?[], object> initializer = builder.Command.Module.InteractionService._useCompiledLambda
                        ? ReflectionUtils<object>.CreateLambdaConstructorInvoker(paramInfo.ParameterType.GetTypeInfo())
                        : ctor.Invoke;
                    builder.ComplexParameterInitializer = args => initializer(args);
                }
                    break;
                default:
                    builder.AddAttributes(attribute);
                    break;
            }
        }

        builder.SetParameterType(paramType, services);

        // Replace pascal casings with '-'
        if (builder.Name != null)
            builder.Name = Regex.Replace(builder.Name, "(?<=[a-z])(?=[A-Z])", "-").ToLower();
    }

    private static void BuildParameter<TInfo, TBuilder>(ParameterBuilder<TInfo, TBuilder> builder, ParameterInfo paramInfo)
        where TInfo : class, IParameterInfo
        where TBuilder : ParameterBuilder<TInfo, TBuilder>
    {
        IEnumerable<Attribute> attributes = paramInfo.GetCustomAttributes();
        Type paramType = paramInfo.ParameterType;

        builder.Name = paramInfo.Name;
        builder.IsRequired = !paramInfo.IsOptional;
        builder.DefaultValue = paramInfo.DefaultValue;
        builder.SetParameterType(paramType);

        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case ParameterPreconditionAttribute precondition:
                    builder.AddPreconditions(precondition);
                    break;
                case ParamArrayAttribute:
                    builder.IsParameterArray = true;
                    break;
                default:
                    builder.AddAttributes(attribute);
                    break;
            }
        }
    }
    #endregion

    internal static bool IsValidModuleDefinition(TypeInfo typeInfo)
    {
        return ModuleTypeInfo.IsAssignableFrom(typeInfo) &&
            !typeInfo.IsAbstract &&
            !typeInfo.ContainsGenericParameters;
    }

    private static bool IsValidSlashCommandDefinition(MethodInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(SlashCommandAttribute)) &&
            (methodInfo.ReturnType == typeof(Task) || methodInfo.ReturnType == typeof(Task<RuntimeResult>)) &&
            !methodInfo.IsStatic &&
            !methodInfo.IsGenericMethod;
    }

    private static ConstructorInfo GetComplexParameterConstructor(TypeInfo typeInfo, ComplexParameterAttribute complexParameter)
    {
        ConstructorInfo[] ctors = typeInfo.GetConstructors();

        if (ctors.Length == 0)
            throw new InvalidOperationException($"No constructor found for \"{typeInfo.FullName}\".");

        if (complexParameter.PrioritizedCtorSignature is not null)
        {
            ConstructorInfo? ctor = typeInfo.GetConstructor(complexParameter.PrioritizedCtorSignature);

            if (ctor is null)
                throw new InvalidOperationException($"No constructor was found with the signature: {string.Join(",", complexParameter.PrioritizedCtorSignature.Select(x => x.Name))}");

            return ctor;
        }

        List<ConstructorInfo> prioritizedCtors = ctors
            .Where(x => x.IsDefined(typeof(ComplexParameterCtorAttribute), true))
            .ToList();

        switch (prioritizedCtors.Count)
        {
            case > 1:
                throw new InvalidOperationException($"{nameof(ComplexParameterCtorAttribute)} can only be used once in a type.");
            case 1:
                return prioritizedCtors.First();
        }

        switch (ctors.Length)
        {
            case > 1:
                throw new InvalidOperationException($"Multiple constructors found for \"{typeInfo.FullName}\".");
            default:
                return ctors.First();
        }
    }
}
