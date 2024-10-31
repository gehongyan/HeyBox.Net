using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class ReactionEvent
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("emoji")]
    public required string Emoji { get; set; }

    [JsonPropertyName("is_add")]
    public required ReactionAction Action { get; set; }

    [JsonPropertyName("msg_id")]
    public required ulong MessageId { get; set; }

    [JsonPropertyName("room_id")]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("user_id")]
    public required uint UserId { get; set; }
}

internal enum ReactionAction
{
    Remove = 0,
    Add = 1,
}
