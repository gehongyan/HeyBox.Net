using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class RoomCustomizationQuestionTypesJsonConverter : JsonConverter<RoomCustomizationQuestionTypes>
{
    /// <inheritdoc />
    public override RoomCustomizationQuestionTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string[]? values = JsonSerializer.Deserialize<string[]>(ref reader, options);
        RoomCustomizationQuestionTypes result = RoomCustomizationQuestionTypes.None;
        if (values is null or [])
            return result;
        if (values.Contains("multi"))
            result |= RoomCustomizationQuestionTypes.Multiple;
        if (values.Contains("required"))
            result |= RoomCustomizationQuestionTypes.Required;
        return result;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RoomCustomizationQuestionTypes value, JsonSerializerOptions options)
    {
        List<string> values = [];
        if (value.HasFlag(RoomCustomizationQuestionTypes.Multiple))
            values.Add("multi");
        if (value.HasFlag(RoomCustomizationQuestionTypes.Required))
            values.Add("required");
        JsonSerializer.Serialize(writer, values.ToArray(), options);
    }
}
