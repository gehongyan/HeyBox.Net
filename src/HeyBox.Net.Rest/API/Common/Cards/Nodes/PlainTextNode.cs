using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class PlainTextNode : NodeBase
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}
