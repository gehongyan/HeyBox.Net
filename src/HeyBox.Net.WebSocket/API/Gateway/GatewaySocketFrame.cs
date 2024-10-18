using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API.Gateway;

internal class GatewaySocketFrame
{
    [JsonPropertyName("sequence")]
    public required ulong Sequence { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("notify_type")]
    public required string NotifyType { get; init; }

    [JsonPropertyName("data")]
    public required JsonElement Data { get; init; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public required DateTimeOffset Timestamp { get; init; }
}
