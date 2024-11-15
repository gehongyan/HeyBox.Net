using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class CountdownModule : ModuleBase
{
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(CountdownModeConverter))]
    public required CountdownMode Mode { get; set; }

    [JsonPropertyName("end_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public required DateTimeOffset EndTime { get; set; }
}
