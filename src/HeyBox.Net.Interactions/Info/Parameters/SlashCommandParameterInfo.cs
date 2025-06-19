using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     表示缓存的复杂参数构造委托。
/// </summary>
/// <param name="args"> 方法参数数组。 </param>
/// <returns>
///     返回构造后的对象。
/// </returns>
public delegate object ComplexParameterInitializer(object?[] args);

/// <summary>
///     表示 <see cref="SlashCommandInfo"/> 命令的参数信息类。
/// </summary>
public class SlashCommandParameterInfo : CommandParameterInfo
{
    internal readonly ComplexParameterInitializer? _complexParameterInitializer;

    /// <inheritdoc cref="HeyBox.Interactions.CommandParameterInfo.Command" />
    public new SlashCommandInfo Command => base.Command as SlashCommandInfo
        ?? throw new InvalidOperationException("Command must be a SlashCommandInfo.");

    /// <summary>
    ///     获取斜线命令参数的描述。
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     获取数字类型参数允许的最小值。
    /// </summary>
    public double? MinValue { get; }

    /// <summary>
    ///     获取数字类型参数允许的最大值。
    /// </summary>
    public double? MaxValue { get; }

    /// <summary>
    ///     获取字符串类型参数允许的最小长度。
    /// </summary>
    public int? MinLength { get; }

    /// <summary>
    ///     获取字符串类型参数允许的最大长度。
    /// </summary>
    public int? MaxLength { get; }

    /// <summary>
    ///     获取将用于将传入的 <see cref="HeyBox.IHeyBoxInteractionData"/> 转换为
    ///     <see cref="CommandParameterInfo.ParameterType"/> 的 <see cref="TypeConverter{T}"/>。
    /// </summary>
    public TypeConverter TypeConverter { get; }

    /// <summary>
    ///     获取此类型是否应被视为复杂参数。
    /// </summary>
    public bool IsComplexParameter { get; }

    /// <summary>
    ///     获取此参数所表示的黑盒语音命令选项类型（如果该参数不是复杂参数）。
    /// </summary>
    public SlashCommandOptionType? HeyBoxOptionType => TypeConverter?.GetHeyBoxType();

    /// <summary>
    ///     获取此斜线应用命令参数的参数选择。
    /// </summary>
    public IReadOnlyCollection<ParameterChoice> Choices { get; }

    /// <summary>
    ///     获取此选项允许的频道类型。
    /// </summary>
    public IReadOnlyCollection<ChannelType> ChannelTypes { get; }

    /// <summary>
    ///     获取此参数的构造函数参数（如果 <see cref="IsComplexParameter"/> 为 <see langword="true"/>）。
    /// </summary>
    public IReadOnlyCollection<SlashCommandParameterInfo>? ComplexParameterFields { get; }

    internal SlashCommandParameterInfo(Builders.SlashCommandParameterBuilder builder, SlashCommandInfo command)
        : base(builder, command)
    {
        TypeConverter = builder.TypeConverter ?? throw new InvalidOperationException("TypeConverter must be set.");
        Description = builder.Description;
        MaxValue = builder.MaxValue;
        MinValue = builder.MinValue;
        MinLength = builder.MinLength;
        MaxLength = builder.MaxLength;
        IsComplexParameter = builder.IsComplexParameter;
        Choices = [..builder.Choices];
        ChannelTypes = [..builder.ChannelTypes];
        ComplexParameterFields = builder.ComplexParameterFields?.Select(x => x.Build(command)).ToImmutableArray();

        _complexParameterInitializer = builder.ComplexParameterInitializer;
    }
}
