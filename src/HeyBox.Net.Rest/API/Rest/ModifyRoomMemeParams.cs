using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyRoomMemeParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("path")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Path { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
