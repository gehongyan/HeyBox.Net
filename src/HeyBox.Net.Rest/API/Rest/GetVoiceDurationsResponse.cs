using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetVoiceDurationsResponse
{
    [JsonPropertyName("durations")]
    public GetVoiceDurationItem[]? Durations { get; set; }
}

internal class GetVoiceDurationItem
{
    [JsonPropertyName("room_id")]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("user_id")]
    public required uint UserId { get; set; }

    [JsonPropertyName("create_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("duration")]
    public required int DurationSeconds { get; set; }
}
