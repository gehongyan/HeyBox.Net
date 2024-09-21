using System.Collections.Immutable;
using System.Reflection;

namespace HeyBox.Interactions;

internal sealed class EnumConverter<T> : TypeConverter<T> where T : struct, Enum
{
    public override SlashCommandOptionType GetHeyBoxType() => SlashCommandOptionType.String;
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
        ISlashCommandInteractionDataOption option, IServiceProvider? services)
    {
        return Task.FromResult(Enum.TryParse(option.RawValue, out T result)
            ? TypeConverterResult.FromSuccess(result)
            : TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Value {option.Value} cannot be converted to {nameof(T)}"));
    }

    public override void Write(SlashCommandOptionProperties properties, IParameterInfo parameterInfo)
    {
        var names = Enum.GetNames(typeof(T));
        var members = names
            .SelectMany(x => typeof(T).GetMember(x)).Where(x => !x.IsDefined(typeof(HideAttribute), true))
            .ToList();

        if (members.Count() <= 25)
        {
            var choices = new List<SlashCommandOptionChoiceProperties>();

            foreach (var member in members)
            {
                var displayValue = member.GetCustomAttribute<ChoiceDisplayAttribute>()?.Name ?? member.Name;
                choices.Add(new SlashCommandOptionChoiceProperties
                {
                    Name = displayValue,
                    Value = member.Name,
                });
            }
            properties.Choices = choices;
        }
    }
}

/// <summary>
///     Enum values tagged with this attribute will not be displayed as a parameter choice
/// </summary>
/// <remarks>
///     This attribute must be used along with the default <see cref="EnumConverter{T}"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class HideAttribute : Attribute { }
