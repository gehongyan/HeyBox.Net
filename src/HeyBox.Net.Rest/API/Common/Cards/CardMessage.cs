using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class CardMessage
{
    [JsonPropertyName("data")]
    public required CardBase[] Data { get; set; }
}
