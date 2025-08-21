using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class BeginAudioStreamTaskParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("stream_url")]
    public required string StreamUrl { get; set; }

    [JsonPropertyName("volume")]
    public int? Volume { get; set; }

    [JsonPropertyName("Operator")]
    public required int Operator { get; set; }

    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("seek_second")]
    public int? SeekSecond { get; set; }

    [JsonPropertyName("repeat_num")]
    public int? RepeatNum { get; set; }

    [JsonPropertyName("max_duration")]
    public int? MaxDuration { get; set; }
}
