using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class SendChannelMessageResponse
{
    [JsonPropertyName("chatmobile_ack_id")]
    public required string ChatMobileAckId { get; init; }

    [JsonPropertyName("heychat_ack_id")]
    public required string HeyChatAckId { get; init; }

    [JsonPropertyName("msg_id")]
    public required ulong MessageId { get; init; }

    [JsonPropertyName("msg_seq")]
    public required ulong MessageSequence { get; init; }
}
