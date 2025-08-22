using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomCustomizationQuestion
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong Id { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("question_type")]
    [JsonConverter(typeof(RoomCustomizationQuestionTypesJsonConverter))]
    public RoomCustomizationQuestionTypes QuestionType { get; set; }

    [JsonPropertyName("answers")]
    public required RoomCustomizationAnswer[] Answers { get; set; }
}