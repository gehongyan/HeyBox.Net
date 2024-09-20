using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API.Gateway;

internal class CommandEvent
{
    [JsonPropertyName("bot_id")]
    public required int BotId { get; set; }

    [JsonPropertyName("channel_base_info")]
    public required ChannelBaseInfo ChannelBaseInfo { get; set; }

    [JsonPropertyName("command_info")]
    public required CommandInfo CommandInfo { get; set; }

    [JsonPropertyName("msg_id")]
    public required ulong MessageId { get; set; }

    [JsonPropertyName("room_base_info")]
    public required RoomBaseInfo RoomBaseInfo { get; set; }

    [JsonPropertyName("send_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.TimestampUnit.Milliseconds)]
    public required DateTimeOffset SendTime { get; set; }

    [JsonPropertyName("sender_info")]
    public required SenderInfo SenderInfo { get; set; }
}
