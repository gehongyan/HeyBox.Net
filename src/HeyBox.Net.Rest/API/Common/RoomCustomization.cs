using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomCustomization
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("customization_switch")]
    public bool CustomizationSwitch { get; set; }

    [JsonPropertyName("available_channels")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong[]? AvailableChannels { get; set; }

    [JsonPropertyName("questions")]
    public RoomCustomizationQuestion[]? Questions { get; set; }

    [JsonPropertyName("guidance")]
    public required RoomCustomizationGuidance Guidance { get; set; }

    [JsonPropertyName("update_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public DateTimeOffset? UpdateTime { get; set; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public DateTimeOffset? CreateTime { get; set; }

    [JsonPropertyName("available_switch")]
    public bool AvailableSwitch { get; set; }

    [JsonPropertyName("question_switch")]
    public bool QuestionSwitch { get; set; }

    [JsonPropertyName("guidance_switch")]
    public bool GuidanceSwitch { get; set; }
}
