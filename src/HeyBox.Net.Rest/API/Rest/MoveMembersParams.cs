using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class MoveMembersParams
{
    [JsonPropertyName("origin_channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong OriginalChannelId { get; set; }

    [JsonPropertyName("to_user_ids")]
    public required string[] ToUserIds { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; set; }
}
