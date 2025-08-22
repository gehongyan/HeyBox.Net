using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomCustomizationGuidance
{
    [JsonPropertyName("resource")]
    public required RoomCustomizationGuidanceResource[] Resource { get; set; }

    [JsonPropertyName("banner")]
    public required string Banner { get; set; }

    [JsonPropertyName("banner_color")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public required Color BannerColor { get; set; }

    [JsonPropertyName("protocol")]
    public required string Protocol { get; set; }
}
