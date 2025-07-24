using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class BanOperationParams
{
    [JsonPropertyName("duration")]
    public required ulong DurationSeconds { get; set; }

    [JsonPropertyName("reason")]
    public required string Reason { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("to_user_id")]
    public required ulong ToUserId { get; set; }
}
