using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class BeginAudioStreamTaskResponse
{
    [JsonPropertyName("task_id")]
    public required string TaskId { get; set; }
}
