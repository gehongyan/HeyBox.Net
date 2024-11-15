using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class NodeTypeConverter : JsonConverter<NodeType>
{
    public override NodeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? type = reader.GetString();
        return type switch
        {
            "plain-text" => NodeType.PlainText,
            "markdown" => NodeType.Markdown,
            "image" => NodeType.Image,
            "button" => NodeType.Button,
            _ => throw new ArgumentOutOfRangeException(nameof(CardType))
        };
    }

    public override void Write(Utf8JsonWriter writer, NodeType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            NodeType.PlainText => "plain-text",
            NodeType.Markdown => "markdown",
            NodeType.Image => "image",
            NodeType.Button => "button",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
