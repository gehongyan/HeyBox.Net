using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        new(reader.GetUInt32());

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.RawValue);
}

internal class NullableColorConverter : JsonConverter<Color?>
{
    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null ? null : new Color(reader.GetUInt32());

    public override void Write(Utf8JsonWriter writer, Color? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteNumberValue(value.Value.RawValue);
        else
            writer.WriteNullValue();
    }
}
