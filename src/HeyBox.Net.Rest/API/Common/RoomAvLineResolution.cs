using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomAvLineResolution
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("bitrate")]
    public int Bitrate { get; set; }

    [JsonPropertyName("min_bitrate")]
    public int MinBitrate { get; set; }
}