using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class RoleGradientColorConverter : JsonConverter<GradientColor?>
{
    /// <inheritdoc />
    public override GradientColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        uint[]? values = JsonSerializer.Deserialize<uint[]?>(ref reader, options);
        if (values is [var left, var right])
            return new GradientColor(new Color(left), new Color(right));
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
