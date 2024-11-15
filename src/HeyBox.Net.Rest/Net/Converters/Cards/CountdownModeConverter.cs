using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class CountdownModeConverter : JsonConverter<CountdownMode>
{
    public override CountdownMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? mode = reader.GetString();
        return mode switch
        {
            "default" => CountdownMode.Default,
            "calendar" => CountdownMode.Calendar,
            "second" => CountdownMode.Second,
            _ => throw new ArgumentOutOfRangeException(nameof(CountdownMode))
        };
    }

    public override void Write(Utf8JsonWriter writer, CountdownMode value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            CountdownMode.Default => "default",
            CountdownMode.Calendar => "calendar",
            CountdownMode.Second => "second",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
}
