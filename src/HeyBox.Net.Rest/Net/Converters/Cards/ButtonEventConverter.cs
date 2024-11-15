using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ButtonEventConverter : JsonConverter<ButtonEvent>
{
    public override ButtonEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? type = reader.GetString();
        return type switch
        {
            "link-to" => ButtonEvent.LinkTo,
            "server" => ButtonEvent.Server,
            _ => throw new ArgumentOutOfRangeException(nameof(ButtonEvent))
        };
    }

    public override void Write(Utf8JsonWriter writer, ButtonEvent value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ButtonEvent.LinkTo => "link-to",
            ButtonEvent.Server => "server",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
