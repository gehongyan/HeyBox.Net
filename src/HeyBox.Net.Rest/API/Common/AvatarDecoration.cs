using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class AvatarDecoration
{
    [JsonPropertyName("src_type")]
    public required string SrcType { get; set; }

    [JsonPropertyName("src_url")]
    public required string SrcUrl { get; set; }
}
