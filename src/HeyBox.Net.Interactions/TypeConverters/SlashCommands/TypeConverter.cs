namespace HeyBox.Interactions;

/// <summary> 类型转换器基类。<see cref="InteractionService"/> 使用 TypeConverter 处理斜线命令参数。 </summary>
public abstract class TypeConverter : ITypeConverter<ISlashCommandInteractionDataOption>
{
    /// <summary> 当命令服务遇到未知参数类型时，将用于查找可用的 TypeConverter。 </summary>
    /// <param name="type"> 参数类型。 </param>
    /// <returns> 是否可以转换。 </returns>
    public abstract bool CanConvertTo(Type type);

    /// <summary> 获取应用命令选项类型。 </summary>
    /// <returns> 选项类型。 </returns>
    public abstract SlashCommandOptionType GetHeyBoxType();

    /// <summary> 在执行方法体前读取传入的参数。 </summary>
    /// <param name="context"> 命令执行上下文。 </param>
    /// <param name="option"> 接收到的选项参数。 </param>
    /// <param name="services"> 用于初始化命令模块的服务提供器。 </param>
    /// <returns> 读取过程的结果。 </returns>
    public abstract Task<TypeConverterResult> ReadAsync(IInteractionContext context,
        ISlashCommandInteractionDataOption option, IServiceProvider? services);

    /// <summary> 在命令注册到 HeyBox 前，用于操作输出的命令选项。 </summary>
    public virtual void Write(SlashCommandOptionProperties properties, IParameterInfo parameter) { }
}

/// <inheritdoc/>
public abstract class TypeConverter<T> : TypeConverter
{
    /// <inheritdoc/>
    public sealed override bool CanConvertTo(Type type) =>
        typeof(T).IsAssignableFrom(type);
}
