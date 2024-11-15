using HeyBox.API;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ModuleConverter : JsonConverter<ModuleBase>
{
    public static readonly ModuleConverter Instance = new();

    public override ModuleBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonNode? jsonNode = JsonNode.Parse(ref reader);
        return jsonNode?["type"]?.GetValue<string>() switch
        {
            "section" => JsonSerializer.Deserialize<API.SectionModule>(jsonNode.ToJsonString(), options),
            "header" => JsonSerializer.Deserialize<API.HeaderModule>(jsonNode.ToJsonString(), options),
            "images" => JsonSerializer.Deserialize<API.ImagesModule>(jsonNode.ToJsonString(), options),
            "button-group" => JsonSerializer.Deserialize<API.ButtonGroupModule>(jsonNode.ToJsonString(), options),
            "divider" => JsonSerializer.Deserialize<API.DividerModule>(jsonNode.ToJsonString(), options),
            "countdown" => JsonSerializer.Deserialize<API.CountdownModule>(jsonNode.ToJsonString(), options),
            _ => throw new ArgumentOutOfRangeException(nameof(CardType))
        };
    }

    public override void Write(Utf8JsonWriter writer, ModuleBase value, JsonSerializerOptions options)
    {
        string json = value switch
        {
            API.HeaderModule header => JsonSerializer.Serialize(header, options),
            API.SectionModule section => JsonSerializer.Serialize(section, options),
            API.ImagesModule images => JsonSerializer.Serialize(images, options),
            API.ButtonGroupModule buttonGroup => JsonSerializer.Serialize(buttonGroup, options),
            API.DividerModule divider => JsonSerializer.Serialize(divider, options),
            API.CountdownModule countdown => JsonSerializer.Serialize(countdown, options),
            _ => throw new ArgumentOutOfRangeException(nameof(value.Type), value, "Unknown module type")
        };
        writer.WriteRawValue(json);
    }
}
