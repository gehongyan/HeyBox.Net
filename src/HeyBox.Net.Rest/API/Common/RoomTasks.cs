using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomTasks
{
    [JsonPropertyName("room_level")]
    public int RoomLevel { get; set; }

    [JsonPropertyName("finish_rate")]
    public double FinishRate { get; set; }

    [JsonPropertyName("now_level_finish_rate")]
    public double NowLevelFinishRate { get; set; }

    [JsonPropertyName("max_room_level")]
    public int MaxRoomLevel { get; set; }

    [JsonPropertyName("tasks")]
    public required RoomTask[] Tasks { get; set; }

    [JsonPropertyName("warn_tasks")]
    public object? WarnTasks { get; set; } // TODO
}