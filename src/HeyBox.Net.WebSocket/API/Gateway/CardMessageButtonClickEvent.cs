using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API.Gateway;

internal class CardMessageButtonClickEvent
{
    [JsonPropertyName("channel_base_info")]
    public required ChannelBaseInfo ChannelBaseInfo { get; set; }

    [JsonPropertyName("event")]
    [JsonConverter(typeof(ButtonEventConverter))]
    public required ButtonEvent Event { get; set; }

    [JsonPropertyName("msg_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong MessageId { get; set; }

    [JsonPropertyName("os_type")]
    public required string ClientType { get; set; }

    [JsonPropertyName("room_base_info")]
    public required RoomBaseInfo RoomBaseInfo { get; set; }

    [JsonPropertyName("send_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public required DateTimeOffset MessageSendTime { get; set; }

    [JsonPropertyName("sender_info")]
    public required RoomUser SenderInfo { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}
