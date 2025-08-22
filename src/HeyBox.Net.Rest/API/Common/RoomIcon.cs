using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomIcon
{
    [JsonPropertyName("room_icon_id")]
    public int RoomIconId { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}