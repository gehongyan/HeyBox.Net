using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class MuteDeafenUserInRoomParams
{
    [JsonPropertyName("to_user_id")]
    public required ulong ToUserId { get; set; }

    [JsonPropertyName("mute")]
    public required bool Mute { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }
}
