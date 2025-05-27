using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class Card : CardBase
{
    [JsonPropertyName("border_color")]
    public string? BorderColor { get; set; }

    [JsonPropertyName("size")]
    [JsonConverter(typeof(CardSizeConverter))]
    public CardSize? Size { get; set; }

    [JsonPropertyName("modules")]
    public required ModuleBase[] Modules { get; set; }
}
