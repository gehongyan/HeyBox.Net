using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomFireFlag
{
    [JsonPropertyName("cumulative_days")]
    public int CumulativeDays { get; set; }

    [JsonPropertyName("continuous_days")]
    public int ContinuousDays { get; set; }

    [JsonPropertyName("active_members")]
    public int ActiveMembers { get; set; }

    [JsonPropertyName("inactive_days")]
    public int InactiveDays { get; set; }
}