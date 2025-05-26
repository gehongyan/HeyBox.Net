using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class SendUserMessageParams
{
    [JsonPropertyName("msg_type")]
    public required MessageType MessageType { get; init; }

    [JsonPropertyName("msg")]
    public required string Message { get; init; }

    [JsonPropertyName("heychat_ack_id")]
    public string HeyChatAckId => GetHashCode().ToString();

    [JsonPropertyName("addition")]
    public required string Addition { get; init; }

    [JsonPropertyName("to_user_id")]
    public required ulong ToUserId { get; set; }
}
