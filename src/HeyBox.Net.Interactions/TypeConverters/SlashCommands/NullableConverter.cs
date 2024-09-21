using System;
using System.Threading.Tasks;

namespace HeyBox.Interactions
{
    internal class NullableConverter<T> : TypeConverter<T>
    {
        private readonly TypeConverter _typeConverter;

        public NullableConverter(InteractionService interactionService, IServiceProvider services)
        {
            var nullableType = typeof(T);
            var type = Nullable.GetUnderlyingType(nullableType);

            if (type is null)
                throw new ArgumentException($"No type {nameof(TypeConverter)} is defined for this {nullableType.FullName}", nameof(type));

            _typeConverter = interactionService.GetTypeConverter(type, services);
        }

        public override SlashCommandOptionType GetHeyBoxType()
            => _typeConverter.GetHeyBoxType();

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
            ISlashCommandInteractionDataOption option, IServiceProvider? services)
            => _typeConverter.ReadAsync(context, option, services);

        public override void Write(SlashCommandOptionProperties properties, IParameterInfo parameter)
            => _typeConverter.Write(properties, parameter);
    }
}
