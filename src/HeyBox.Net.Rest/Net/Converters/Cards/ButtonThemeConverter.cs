using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class ButtonThemeConverter : JsonConverter<ButtonTheme>
{
    public override ButtonTheme Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? theme = reader.GetString();
        return theme switch
        {
            "default" => ButtonTheme.Default,
            "primary" => ButtonTheme.Primary,
            "success" => ButtonTheme.Success,
            "danger" => ButtonTheme.Danger,
            _ => throw new ArgumentOutOfRangeException(nameof(ButtonTheme))
        };
    }

    public override void Write(Utf8JsonWriter writer, ButtonTheme value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ButtonTheme.Default => "default",
            ButtonTheme.Primary => "primary",
            ButtonTheme.Success => "success",
            ButtonTheme.Danger => "danger",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
