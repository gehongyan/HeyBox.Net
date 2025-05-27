using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class NodeBase : INode
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(NodeTypeConverter))]
    public required NodeType Type { get; set; }

    [JsonPropertyName("width")]
    public string? Width { get; set; }
}
