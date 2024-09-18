using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Rest.Converters;

internal class HeyBoxErrorCodeJsonConverter : JsonConverter<HeyBoxErrorCode>
{
    /// <inheritdoc />
    public override HeyBoxErrorCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? code = reader.GetString();
        if (code is null)
            throw new JsonException("The error code is null.");
        return new HeyBoxErrorCode(code);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, HeyBoxErrorCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.RawCode);
    }
}
