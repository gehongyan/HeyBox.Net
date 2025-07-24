using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     表示基于特性的方法信息类，适用于 <see cref="ApplicationCommandType.Slash"/> 类型的命令。
/// </summary>
public class SlashCommandInfo : CommandInfo<SlashCommandParameterInfo>, IApplicationCommandInfo
{
    internal IReadOnlyDictionary<string, SlashCommandParameterInfo> _flattenedParameterDictionary { get; }

    /// <summary>
    ///     获取将在黑盒语音上显示的命令描述。
    /// </summary>
    public string? Description { get; }

    /// <inheritdoc/>
    public ApplicationCommandType CommandType { get; } = ApplicationCommandType.Slash;

    /// <inheritdoc/>
    public override IReadOnlyList<SlashCommandParameterInfo> Parameters { get; }

    /// <summary>
    ///     获取命令参数及复杂参数字段的扁平集合。
    /// </summary>
    public IReadOnlyList<SlashCommandParameterInfo> FlattenedParameters { get; }

    /// <inheritdoc/>
    public IReadOnlyCollection<InteractionContextType> ContextTypes { get; }

    internal SlashCommandInfo(Builders.SlashCommandBuilder builder, ModuleInfo module, InteractionService commandService)
        : base(builder, module, commandService)
    {
        Description = builder.Description;
        Parameters = builder.Parameters.Select(x => x.Build(this)).ToImmutableArray();
        FlattenedParameters = FlattenParameters(Parameters).ToImmutableArray();
        ContextTypes = builder.ContextTypes?.ToImmutableArray() ?? throw new InvalidOperationException("Slash commands must have context types.");

        for (int i = 0; i < FlattenedParameters.Count - 1; i++)
        {
            if (!FlattenedParameters.ElementAt(i).IsRequired && FlattenedParameters.ElementAt(i + 1).IsRequired)
                throw new InvalidOperationException("Optional parameters must appear after all required parameters, ComplexParameters with optional parameters must be located at the end.");
        }

        _flattenedParameterDictionary = FlattenedParameters.ToDictionary(x => x.Name, x => x).ToImmutableDictionary();
    }

    /// <inheritdoc/>
    public override Task<IResult> ExecuteAsync(IInteractionContext context, IServiceProvider? services)
    {
        if (context.Interaction is not ISlashCommandInteraction)
            return Task.FromResult((IResult)ExecuteResult.FromError(InteractionCommandError.ParseFailed, $"Provided {nameof(IInteractionContext)} doesn't belong to a Slash Command Interaction"));

        return base.ExecuteAsync(context, services);
    }

    /// <summary>
    ///     解析斜线命令参数。
    /// </summary>
    /// <param name="context"> 命令上下文。 </param>
    /// <param name="services"> 服务集合。 </param>
    /// <returns> 解析结果。 </returns>
    protected override async Task<IResult> ParseArgumentsAsync(IInteractionContext context, IServiceProvider? services)
    {
        List<ISlashCommandInteractionDataOption>? GetOptions()
        {
            IReadOnlyCollection<ISlashCommandInteractionDataOption>? options = (context.Interaction as ISlashCommandInteraction)?.Data.Options;
            return options?.ToList();
        }

        var options = GetOptions();
        var args = new object?[Parameters.Count];
        for (var i = 0; i < Parameters.Count; i++)
        {
            var parameter = Parameters[i];
            var result = await ParseArgumentAsync(parameter, context, options, services).ConfigureAwait(false);

            if (!result.IsSuccess)
                return ParseResult.FromError(result);

            if (result is not TypeConverterResult { IsSuccess: true } converterResult)
                return ExecuteResult.FromError(InteractionCommandError.BadArgs, "Complex command parsing failed for an unknown reason.");

            args[i] = converterResult.Value;
        }
        return ParseResult.FromSuccess(args);
    }

    private async ValueTask<IResult> ParseArgumentAsync(SlashCommandParameterInfo parameterInfo,
        IInteractionContext context, List<ISlashCommandInteractionDataOption>? argList, IServiceProvider? services)
    {
        if (parameterInfo.IsComplexParameter && parameterInfo.ComplexParameterFields is not null)
        {
            var ctorArgs = new object?[parameterInfo.ComplexParameterFields.Count];

            for (var i = 0; i < ctorArgs.Length; i++)
            {
                var result = await ParseArgumentAsync(parameterInfo.ComplexParameterFields.ElementAt(i), context, argList, services).ConfigureAwait(false);

                if (!result.IsSuccess)
                    return result;

                if (result is not TypeConverterResult converterResult)
                    return ExecuteResult.FromError(InteractionCommandError.BadArgs, "Complex command parsing failed for an unknown reason.");

                ctorArgs[i] = converterResult.Value;
            }

            return TypeConverterResult.FromSuccess(parameterInfo._complexParameterInitializer?.Invoke(ctorArgs));
        }

        var arg = argList?.Find(x => string.Equals(x.Name, parameterInfo.Name, StringComparison.OrdinalIgnoreCase));

        if (arg == default)
            return parameterInfo.IsRequired ? ExecuteResult.FromError(InteractionCommandError.BadArgs, "Command was invoked with too few parameters") :
                TypeConverterResult.FromSuccess(parameterInfo.DefaultValue);

        var typeConverter = parameterInfo.TypeConverter;
        var readResult = await typeConverter.ReadAsync(context, arg, services).ConfigureAwait(false);
        return readResult;
    }

    /// <summary>
    ///     调用斜线命令事件。
    /// </summary>
    /// <param name="context"> 命令上下文。 </param>
    /// <param name="result"> 命令执行结果。 </param>
    /// <returns> 异步任务。 </returns>
    protected override Task InvokeModuleEvent(IInteractionContext context, IResult result)
        => CommandService._slashCommandExecutedEvent.InvokeAsync(this, context, result);

    /// <summary>
    ///     获取斜线命令的日志字符串。
    /// </summary>
    /// <param name="context"> 命令上下文。 </param>
    /// <returns> 日志字符串。 </returns>
    protected override string GetLogString(IInteractionContext context) =>
        context.Room != null
            ? $"Slash Command: \"{base.ToString()}\" for {context.User} in {context.Room}/{context.Channel}"
            : $"Slash Command: \"{base.ToString()}\" for {context.User} in {context.Channel}";

    private static IEnumerable<SlashCommandParameterInfo> FlattenParameters(IEnumerable<SlashCommandParameterInfo> parameters)
    {
        foreach (SlashCommandParameterInfo parameter in parameters)
        {
            if (!parameter.IsComplexParameter)
                yield return parameter;
            else if (parameter.ComplexParameterFields != null)
            {
                foreach (SlashCommandParameterInfo complexParameterField in parameter.ComplexParameterFields)
                    yield return complexParameterField;
            }
        }
    }
}
