using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class Medal
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("nameShort")]
    public required string NameShort { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("color")]
    public required string Color { get; set; }

    [JsonPropertyName("img_url")]
    public required string ImageUrl { get; set; }

    [JsonPropertyName("medal_id")]
    public required ulong MedalId { get; set; }
}
