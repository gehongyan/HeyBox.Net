using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class GradientColorConverter : JsonConverter<GradientColor?>
{
    /// <inheritdoc />
    public override GradientColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null) return null;
        // [123, 345]
        if (reader.TokenType is not JsonTokenType.StartArray)
            throw new JsonException($"{nameof(GradientColorConverter)} expects star array token, but got {reader.TokenType}");
        reader.Read();
        if (reader.TokenType is not JsonTokenType.Number)
            throw new JsonException($"{nameof(GradientColorConverter)} expects number token, but got {reader.TokenType}");
        uint left = reader.GetUInt32();
        reader.Read();
        if (reader.TokenType is not JsonTokenType.Number)
            throw new JsonException($"{nameof(GradientColorConverter)} expects number token, but got {reader.TokenType}");
        uint right = reader.GetUInt32();
        reader.Read();
        if (reader.TokenType is not JsonTokenType.EndArray)
            throw new JsonException($"{nameof(GradientColorConverter)} expects end array token, but got {reader.TokenType}");
        return new GradientColor(new Color(left), new Color(right));
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GradientColor? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteNullValue();
            return;
        }
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Value.Left.RawValue);
        writer.WriteNumberValue(value.Value.Right.RawValue);
        writer.WriteEndArray();
    }
}
