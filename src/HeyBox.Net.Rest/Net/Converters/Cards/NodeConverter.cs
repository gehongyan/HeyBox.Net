using HeyBox.API;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class NodeConverter : JsonConverter<NodeBase>
{
    public static readonly NodeConverter Instance = new();

    public override NodeBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonNode? jsonNode = JsonNode.Parse(ref reader);
        return jsonNode?["type"]?.GetValue<string>() switch
        {
            "plain-text" => JsonSerializer.Deserialize<API.PlainTextNode>(jsonNode.ToJsonString(), options),
            "markdown" => JsonSerializer.Deserialize<API.MarkdownNode>(jsonNode.ToJsonString(), options),
            "image" => JsonSerializer.Deserialize<API.ImageNode>(jsonNode.ToJsonString(), options),
            "button" => JsonSerializer.Deserialize<API.ButtonNode>(jsonNode.ToJsonString(), options),
            _ => throw new ArgumentOutOfRangeException(nameof(NodeType))
        };
    }

    public override void Write(Utf8JsonWriter writer, NodeBase value, JsonSerializerOptions options)
    {
        string json = value switch
        {
            API.PlainTextNode plainTextNode => JsonSerializer.Serialize(plainTextNode, options),
            API.MarkdownNode markdownNode => JsonSerializer.Serialize(markdownNode, options),
            API.ImageNode imageNode => JsonSerializer.Serialize(imageNode, options),
            API.ButtonNode buttonNode => JsonSerializer.Serialize(buttonNode, options),
            _ => throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, "Unknown node type")
        };
        writer.WriteRawValue(json);
    }
}
