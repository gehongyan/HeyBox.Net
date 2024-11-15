using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ModuleTypeConverter : JsonConverter<ModuleType>
{
    public override ModuleType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? type = reader.GetString();
        return type switch
        {
            "section" => ModuleType.Section,
            "header" => ModuleType.Header,
            "images" => ModuleType.Images,
            "button-group" => ModuleType.ButtonGroup,
            "divider" => ModuleType.Divider,
            "countdown" => ModuleType.Countdown,
            _ => throw new ArgumentOutOfRangeException(nameof(CardType))
        };
    }

    public override void Write(Utf8JsonWriter writer, ModuleType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ModuleType.Section => "section",
            ModuleType.Header => "header",
            ModuleType.Images => "images",
            ModuleType.ButtonGroup => "button-group",
            ModuleType.Divider => "divider",
            ModuleType.Countdown => "countdown",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
