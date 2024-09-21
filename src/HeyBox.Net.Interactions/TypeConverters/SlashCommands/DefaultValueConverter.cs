namespace HeyBox.Interactions;

internal class DefaultValueConverter<T> : TypeConverter<T> where T : IConvertible
{
    public override SlashCommandOptionType GetHeyBoxType()
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Boolean:
                return SlashCommandOptionType.Boolean;

            case TypeCode.DateTime:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Char:
            case TypeCode.String:
            case TypeCode.Single:
                return SlashCommandOptionType.String;

            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return SlashCommandOptionType.Integer;

            // case TypeCode.Decimal:
            // case TypeCode.Double:
            //     return SlashCommandOptionType.Number;

            case TypeCode.DBNull:
            default:
                throw new InvalidOperationException($"Parameter Type {typeof(T).FullName} is not supported by HeyBox.");
        }
    }
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
        ISlashCommandInteractionDataOption option, IServiceProvider? services)
    {
        object? value = option.Value;

        try
        {
            var converted = Convert.ChangeType(value, typeof(T));
            return Task.FromResult(TypeConverterResult.FromSuccess(converted));
        }
        catch (InvalidCastException castEx)
        {
            return Task.FromResult(TypeConverterResult.FromError(castEx));
        }
    }
}
