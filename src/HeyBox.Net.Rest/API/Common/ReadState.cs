using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class ReadState
{
    [JsonPropertyName("message_count")]
    public int MessageCount { get; set; }

    [JsonPropertyName("mention_count")]
    public int MentionCount { get; set; }

    [JsonPropertyName("last_read_message_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong LastReadMessageId { get; set; }

    [JsonPropertyName("last_read_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public DateTimeOffset? LastReadTime { get; set; }

    [JsonPropertyName("last_at_message_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong LastAtMessageId { get; set; }
}
