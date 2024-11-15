using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ImageSizeConverter : JsonConverter<ImageSize>
{
    public override ImageSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? size = reader.GetString();
        return size switch
        {
            "large" => ImageSize.Large,
            "medium" => ImageSize.Medium,
            "small" => ImageSize.Small,
            _ => throw new ArgumentOutOfRangeException(nameof(ImageSize))
        };
    }

    public override void Write(Utf8JsonWriter writer, ImageSize value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ImageSize.Large => "large",
            ImageSize.Medium => "medium",
            ImageSize.Small => "small",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
