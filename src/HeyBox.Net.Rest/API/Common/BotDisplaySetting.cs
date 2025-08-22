using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotDisplaySetting
{
    [JsonPropertyName("bind_app_ids")]
    public int[]? BindAppIds { get; set; }

    [JsonPropertyName("categories")]
    public BotDisplayCategory[]? Categories { get; set; }

    [JsonPropertyName("icon")]
    public RoomIcon[]? Icon { get; set; }
}