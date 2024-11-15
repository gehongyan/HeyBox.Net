using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class ImageNode : NodeBase
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("size")]
    [JsonConverter(typeof(ImageSizeConverter))]
    public ImageSize? Size { get; set; }
}
