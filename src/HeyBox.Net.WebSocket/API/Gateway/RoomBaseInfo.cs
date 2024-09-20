using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class RoomBaseInfo
{
    [JsonPropertyName("room_avatar")]
    public required string RoomAvatar { get; set; }

    [JsonPropertyName("room_id")]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("room_name")]
    public required string RoomName { get; set; }
}