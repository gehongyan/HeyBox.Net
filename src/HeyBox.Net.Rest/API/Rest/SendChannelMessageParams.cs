using System.Text.Json.Serialization;
using HeyBox;
using HeyBox.Rest.Converters;

namespace HeyBox.API.Rest;

internal class SendChannelMessageParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; init; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("msg_type")]
    public required MessageType MsgType { get; init; }

    [JsonPropertyName("msg")]
    public required string Message { get; init; }

    [JsonPropertyName("heychat_ack_id")]
    public required string HeyChatAckId { get; init; }

    [JsonPropertyName("reply_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong? ReplyId { get; init; }

    [JsonPropertyName("addition")]
    public required string Addition { get; init; }

    [JsonPropertyName("at_user_id")]
    [JsonConverter(typeof(UInt32ArrayStringJoinJsonConverter))]
    public required uint[] AtUserId { get; init; }

    [JsonPropertyName("at_role_id")]
    [JsonConverter(typeof(UInt64ArrayStringJoinJsonConverter))]
    public required ulong[] AtRoleId { get; init; }

    [JsonPropertyName("mention_channel_id")]
    [JsonConverter(typeof(UInt64ArrayStringJoinJsonConverter))]
    public required ulong[] MentionChannelId { get; init; }
}
