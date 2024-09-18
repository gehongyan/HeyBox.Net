using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Rest.Converters;

internal class UInt64ArrayStringJoinJsonConverter : JsonConverter<ulong[]>
{
    /// <inheritdoc />
    public override ulong[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            return null;
        string? str = reader.GetString();
        return str?.Split(',').Select(ulong.Parse).ToArray();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ulong[] value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(string.Join(',', value));
    }
}
