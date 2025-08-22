using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class AutoModFilterWord : AutoModFilterBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("filter_words")]
    public string[]? FilterWords { get; set; }
}