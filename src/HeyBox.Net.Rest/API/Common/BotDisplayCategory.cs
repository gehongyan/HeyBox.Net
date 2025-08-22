using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotDisplayCategory
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
}