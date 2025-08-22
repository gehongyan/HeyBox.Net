using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class AccountTag
{
    [JsonPropertyName("bg_color")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public Color BgColor { get; set; }

    [JsonPropertyName("text_color")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public Color TextColor { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}