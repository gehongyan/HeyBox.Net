using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Rest.Converters;

internal class UInt32ArrayStringJoinJsonConverter : JsonConverter<uint[]>
{
    /// <inheritdoc />
    public override uint[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            return null;
        string? str = reader.GetString();
        return str?.Split(',').Select(uint.Parse).ToArray();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, uint[] value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(string.Join(',', value));
    }
}