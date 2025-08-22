using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomBanThreshold
{
    [JsonPropertyName("days")]
    public int Days { get; set; }

    [JsonPropertyName("threshold")]
    public int Threshold { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("enable")]
    public int Enable { get; set; }
}