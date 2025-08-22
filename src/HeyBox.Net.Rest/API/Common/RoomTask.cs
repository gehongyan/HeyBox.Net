using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomTask
{
    [JsonPropertyName("task_id")]
    public int TaskId { get; set; }

    [JsonPropertyName("task_name")]
    public required string TaskName { get; set; }

    [JsonPropertyName("task_pic")]
    public required string TaskPic { get; set; }

    [JsonPropertyName("target_counts")]
    public int TargetCounts { get; set; }

    [JsonPropertyName("now_counts")]
    public int NowCounts { get; set; }

    [JsonPropertyName("finished")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool Finished { get; set; }

    [JsonPropertyName("reachable")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool Reachable { get; set; }
}