using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class KickOutFromRoomParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("to_user_id")]
    public required ulong ToUserId { get; set; }
}
