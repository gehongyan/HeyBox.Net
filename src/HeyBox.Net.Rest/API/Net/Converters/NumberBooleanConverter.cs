using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class NumberBooleanConverter : JsonConverter<bool>
{
    public BooleanFormat Format { get; }

    /// <inheritdoc />
    internal NumberBooleanConverter(BooleanFormat format)
    {
        Format = format;
    }

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Format switch
        {
            BooleanFormat.Auto => reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False or JsonTokenType.Null => false,
                JsonTokenType.Number => reader.TryGetInt32(out int value) && value == 1,
                JsonTokenType.String => reader.GetString() == "1",
                _ => throw new JsonException(
                    $"{nameof(NumberBooleanConverter)} expects boolean, string or number token, but got {reader.TokenType}")
            },
            BooleanFormat.Boolean => reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False or JsonTokenType.Null => false,
                _ => throw new JsonException(
                    $"{nameof(NumberBooleanConverter)} expects boolean token, but got {reader.TokenType}")
            },
            BooleanFormat.Number => reader.TokenType switch
            {
                JsonTokenType.Number => reader.TryGetInt32(out int value) && value == 1,
                JsonTokenType.Null => false,
                _ => throw new JsonException(
                    $"{nameof(NumberBooleanConverter)} expects number token, but got {reader.TokenType}")
            },
            BooleanFormat.String => reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString() == "1",
                _ => throw new JsonException(
                    $"{nameof(NumberBooleanConverter)} expects string token, but got {reader.TokenType}")
            },
            _ => throw new JsonException($"Unknown {nameof(BooleanFormat)}: {Format}")
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        switch (Format)
        {
            case BooleanFormat.Auto:
            case BooleanFormat.Boolean:
                writer.WriteBooleanValue(value);
                break;
            case BooleanFormat.Number:
                writer.WriteNumberValue(value ? 1 : 0);
                break;
            case BooleanFormat.String:
                writer.WriteStringValue(value ? "1" : "0");
                break;
            default:
                throw new JsonException($"Unknown {nameof(BooleanFormat)}: {Format}");
        }
    }
}

internal class BooleanJsonConverterAttribute : JsonConverterAttribute
{
    public BooleanFormat Format { get; init; }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) => new NumberBooleanConverter(Format);
}

internal enum BooleanFormat
{
    Auto,
    Boolean,
    Number,
    String
}
