using System.Text.Json.Serialization;
using HeyBox.Rest.Converters;

namespace HeyBox.API.Rest;

internal class ReplyReactionParams
{
    [JsonPropertyName("msg_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong MessageId { get; init; }

    [JsonPropertyName("emoji")]
    public required string Emoji { get; init; }

    [JsonPropertyName("is_add")]
    [NumberBooleanConverter(WriteType = NumberBooleanConverter.EnumWriteType.Number)]
    public required bool IsAdd { get; init; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }
}
