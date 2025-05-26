using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class CommandFileInfo
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("option_index")]
    public required int OptionIndex { get; set; }

    [JsonPropertyName("size")]
    public required int Size { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}