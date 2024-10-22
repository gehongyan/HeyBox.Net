using System.Text.Json.Serialization;
using HeyBox.Rest.Converters;

namespace HeyBox.API.Rest;

internal class ModifyChannelMessageParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("msg_type")]
    public required MessageType MessageType { get; init; }

    [JsonPropertyName("msg")]
    public required string Message { get; init; }

    [JsonPropertyName("heychat_ack_id")]
    public string HeyChatAckId => GetHashCode().ToString();

    [JsonPropertyName("reply_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong? ReplyId { get; init; }

    [JsonPropertyName("addition")]
    public required string Addition { get; init; }

    [JsonPropertyName("at_user_id")]
    [JsonConverter(typeof(UInt32ArrayStringJoinJsonConverter))]
    public uint[]? AtUserId { get; init; }

    [JsonPropertyName("at_role_id")]
    [JsonConverter(typeof(UInt64ArrayStringJoinJsonConverter))]
    public ulong[]? AtRoleId { get; init; }

    [JsonPropertyName("mention_channel_id")]
    [JsonConverter(typeof(UInt64ArrayStringJoinJsonConverter))]
    public ulong[]? MentionChannelId { get; init; }

    [JsonPropertyName("msg_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong MessageId { get; set; }
}
