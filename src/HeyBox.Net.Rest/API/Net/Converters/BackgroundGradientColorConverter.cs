using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class BackgroundGradientColorConverter : JsonConverter<GradientColor?>
{
    /// <inheritdoc />
    public override GradientColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
        {
            reader.Skip();
            return null;
        }
        string? value = reader.GetString();
        if (value?.Split(',') is [var left, var right]
            && Color.TryParse(left, out Color leftColor) && Color.TryParse(right, out Color rightColor))
            return new GradientColor(leftColor, rightColor);
        return null;
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
