using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class DividerModule : ModuleBase
{
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }
}
