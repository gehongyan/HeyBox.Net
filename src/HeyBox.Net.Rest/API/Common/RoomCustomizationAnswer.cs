using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomCustomizationAnswer
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong Id { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }

    [JsonPropertyName("channel_ids")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong[]? ChannelIds { get; set; }

    [JsonPropertyName("role_ids")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong[]? RoleIds { get; set; }
}