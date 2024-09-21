namespace HeyBox.Interactions;

/// <summary>
///     用于创建类型转换器的基类。<see cref="InteractionService"/> 使用类型转换器与斜线命令参数进行交互。
/// </summary>
public abstract class TypeReader : ITypeConverter<string>
{
    /// <summary>
    ///     当命令服务遇到未知参数类型时，将使用此方法搜索替代的类型转换器。
    /// </summary>
    /// <param name="type"> 要使用的服务提供程序。 </param>
    /// <returns> 如果找到了替代类型转换器，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public abstract bool CanConvertTo(Type type);

    /// <summary>
    ///     读取字符串并将其转换为指定类型。
    /// </summary>
    /// <param name="context"> 交互上下文。 </param>
    /// <param name="option"> 传入的负载。 </param>
    /// <param name="services"> 要使用的服务提供程序。 </param>
    /// <returns> 一个表示异步读取操作的任务，任务的结果包含转换结果。 </returns>
    public abstract Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option,
        IServiceProvider? services);

    /// <summary>
    ///     将对象序列化为字符串。
    /// </summary>
    /// <param name="obj"> 要序列化的对象。 </param>
    /// <param name="services"> 要使用的服务提供程序。 </param>
    /// <returns> 一个表示异步序列化操作的任务，任务的结果包含序列化结果。 </returns>
    public virtual Task<string?> SerializeAsync(object obj, IServiceProvider services) =>
        Task.FromResult(obj.ToString());
}

/// <inheritdoc/>
public abstract class TypeReader<T> : TypeReader
{
    /// <inheritdoc/>
    public sealed override bool CanConvertTo(Type type) =>
        typeof(T).IsAssignableFrom(type);
}
