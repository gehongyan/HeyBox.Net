using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class MarkdownNode : NodeBase
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}
