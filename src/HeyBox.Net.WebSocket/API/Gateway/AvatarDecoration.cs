using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class AvatarDecoration
{
    [JsonPropertyName("src_type")]
    public required string SourceType { get; set; }

    [JsonPropertyName("src_url")]
    public required string SourceUrl { get; set; }
}