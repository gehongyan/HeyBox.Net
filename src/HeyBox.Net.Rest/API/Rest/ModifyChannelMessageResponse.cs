using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyChannelMessageResponse
{
    [JsonPropertyName("chatmobile_ack_id")]
    public required string ChatMobileAckId { get; init; }

    [JsonPropertyName("heychat_ack_id")]
    public required string HeyChatAckId { get; init; }
}
