using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class HeaderModule : ModuleBase
{
    [JsonPropertyName("content")]
    public required NodeBase Content { get; set; }
}
