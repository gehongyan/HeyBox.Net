using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class DeleteChannelMessageParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("msg_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong MessageId { get; set; }
}
