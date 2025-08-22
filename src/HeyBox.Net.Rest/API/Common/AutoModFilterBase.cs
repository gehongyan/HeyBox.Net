using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class AutoModFilterBase
{
    [JsonPropertyName("actions")]
    public AutoModFilterAction[]? Actions { get; set; }

    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong Id { get; set; }

    [JsonPropertyName("enable")]
    public bool Enable { get; set; }
}