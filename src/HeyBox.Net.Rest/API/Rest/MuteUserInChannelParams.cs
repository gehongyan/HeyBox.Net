using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class MuteUserInChannelParams
{
    [JsonPropertyName("to_user_id")]
    public required ulong ToUserId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }
}
