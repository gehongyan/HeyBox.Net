using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class CreateAssetResponse
{
    [JsonPropertyName("url")]
    public required Uri Url { get; init; }
}
