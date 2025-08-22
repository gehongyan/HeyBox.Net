using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomAvLineList
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("value")]
    [JsonConverter(typeof(VoiceChannelSdkProviderJsonConverter))]
    public required VoiceChannelSdkProvider Value { get; set; }

    [JsonPropertyName("resolution_map")]
    public Dictionary<int, RoomAvLineResolution>? ResolutionMap { get; set; }
}
