using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     Represents a cached argument constructor delegate.
/// </summary>
/// <param name="args">Method arguments array.</param>
/// <returns>
///     Returns the constructed object.
/// </returns>
public delegate object ComplexParameterInitializer(object?[] args);

/// <summary>
///     Represents the parameter info class for <see cref="SlashCommandInfo"/> commands.
/// </summary>
public class SlashCommandParameterInfo : CommandParameterInfo
{
    internal readonly ComplexParameterInitializer? _complexParameterInitializer;

    /// <inheritdoc cref="HeyBox.Interactions.CommandParameterInfo.Command" />
    public new SlashCommandInfo Command => base.Command as SlashCommandInfo
        ?? throw new InvalidOperationException("Command must be a SlashCommandInfo.");

    /// <summary>
    ///     Gets the description of the Slash Command Parameter.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     Gets the minimum value permitted for a number type parameter.
    /// </summary>
    public double? MinValue { get; }

    /// <summary>
    ///     Gets the maximum value permitted for a number type parameter.
    /// </summary>
    public double? MaxValue { get; }

    /// <summary>
    ///     Gets the minimum length allowed for a string type parameter.
    /// </summary>
    public int? MinLength { get; }

    /// <summary>
    ///     Gets the maximum length allowed for a string type parameter.
    /// </summary>
    public int? MaxLength { get; }

    /// <summary>
    ///     Gets the <see cref="TypeConverter{T}"/> that will be used to convert the incoming <see cref="HeyBox.IHeyBoxInteractionData"/> into
    ///     <see cref="CommandParameterInfo.ParameterType"/>.
    /// </summary>
    public TypeConverter TypeConverter { get; }

    /// <summary>
    ///     Gets whether this type should be treated as a complex parameter.
    /// </summary>
    public bool IsComplexParameter { get; }

    /// <summary>
    ///     Gets the HeyBox option type this parameter represents. If the parameter is not a complex parameter.
    /// </summary>
    public SlashCommandOptionType? HeyBoxOptionType => TypeConverter?.GetHeyBoxType();

    /// <summary>
    ///     Gets the parameter choices of this Slash Application Command parameter.
    /// </summary>
    public IReadOnlyCollection<ParameterChoice> Choices { get; }

    /// <summary>
    ///     Gets the allowed channel types for this option.
    /// </summary>
    public IReadOnlyCollection<ChannelType> ChannelTypes { get; }

    /// <summary>
    ///     Gets the constructor parameters of this parameter, if <see cref="IsComplexParameter"/> is <see langword="true"/>.
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
