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
            "internal" => ButtonEvent.Internal,
            "none" => ButtonEvent.None,
            _ => throw new ArgumentOutOfRangeException(nameof(ButtonEvent), type, $"Unknown button event type: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ButtonEvent value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ButtonEvent.LinkTo => "link-to",
            ButtonEvent.Server => "server",
            ButtonEvent.Internal => "internal",
            ButtonEvent.None => "none",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
