using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyChannelSettingParams
{
    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("setting")]
    public required string Setting { get; set; }

    [JsonPropertyName("value")]
    public required int Value { get; set; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; set; }
}
