using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class ButtonNode : NodeBase
{
    [JsonPropertyName("theme")]
    [JsonConverter(typeof(ButtonThemeConverter))]
    public required ButtonTheme Theme { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }

    [JsonPropertyName("event")]
    [JsonConverter(typeof(ButtonEventConverter))]
    public required ButtonEvent Event { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}
