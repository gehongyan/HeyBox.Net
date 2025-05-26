using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class CommandImageInfo
{
    [JsonPropertyName("height")]
    public required int Height { get; set; }

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

    [JsonPropertyName("width")]
    public required int Width { get; set; }
}