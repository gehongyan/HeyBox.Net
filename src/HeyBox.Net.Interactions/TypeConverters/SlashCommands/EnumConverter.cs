using System.Collections.Immutable;
using System.Reflection;

namespace HeyBox.Interactions;

internal sealed class EnumConverter<T> : TypeConverter<T> where T : struct, Enum
{
    public override SlashCommandOptionType GetHeyBoxType() => SlashCommandOptionType.String;
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
        ISlashCommandInteractionDataOption option, IServiceProvider? services)
    {
        if (Enum.TryParse(option.RawValue, out T result))
            return Task.FromResult(TypeConverterResult.FromSuccess(result));
        string[] names = Enum.GetNames(typeof(T));
        foreach (MemberInfo memberInfo in names
                     .SelectMany(x => typeof(T).GetMember(x))
                     .Where(x => !x.IsDefined(typeof(HideAttribute), true)))
        {
            if (memberInfo.GetCustomAttribute<ChoiceValueAttribute>()?.Value == option.RawValue)
                return Task.FromResult(TypeConverterResult.FromSuccess(Enum.Parse<T>(memberInfo.Name)));
        }
        return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed,
            $"Value {option.Value} cannot be converted to {nameof(T)}"));
    }

    public override void Write(SlashCommandOptionProperties properties, IParameterInfo parameterInfo)
    {
        string[] names = Enum.GetNames(typeof(T));
        List<MemberInfo> members = names
            .SelectMany(x => typeof(T).GetMember(x)).Where(x => !x.IsDefined(typeof(HideAttribute), true))
            .ToList();

        if (members.Count() <= 25)
        {
            List<SlashCommandOptionChoiceProperties> choices = new List<SlashCommandOptionChoiceProperties>();

            foreach (MemberInfo member in members)
            {
                string displayValue = member.GetCustomAttribute<ChoiceDisplayAttribute>()?.Name ?? member.Name;
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
