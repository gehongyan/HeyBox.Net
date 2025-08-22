using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomCurrentUserCustomization
{
    [JsonPropertyName("finish")]
    [BooleanJsonConverter(Format = BooleanFormat.Boolean)]
    public bool Finish { get; set; }

    [JsonPropertyName("responses")]
    public object[]? Responses { get; set; } // TODO

    [JsonPropertyName("flush")]
    public bool Flush { get; set; }
}