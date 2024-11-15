using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class SectionModule : ModuleBase
{
    [JsonPropertyName("paragraph")]
    public required NodeBase[] Paragraph { get; init; }
}
