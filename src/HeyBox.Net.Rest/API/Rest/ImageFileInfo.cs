using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ImageFileInfo
{
    [JsonPropertyName("url")]
    public required string? Url { get; init; }

    [JsonPropertyName("width")]
    public required int? Width { get; init; }

    [JsonPropertyName("height")]
    public required int? Height { get; init; }
}
