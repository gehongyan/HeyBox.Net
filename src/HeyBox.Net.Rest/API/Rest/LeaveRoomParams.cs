using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class LeaveRoomParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }
}
