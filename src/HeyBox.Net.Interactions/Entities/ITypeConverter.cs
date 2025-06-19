namespace HeyBox.Interactions;

/// <summary>
///     表示类型转换器接口。
/// </summary>
internal interface ITypeConverter<T>
{
    /// <summary>
    ///     判断是否可以转换为指定类型。
    /// </summary>
    public bool CanConvertTo(Type type);

    /// <summary>
    ///     异步读取并转换类型。
    /// </summary>
    /// <param name="context"> 交互上下文。 </param>
    /// <param name="option"> 选项值。 </param>
    /// <param name="services"> 服务提供器。 </param>
    public Task<TypeConverterResult> ReadAsync(IInteractionContext context, T option, IServiceProvider? services);
}
