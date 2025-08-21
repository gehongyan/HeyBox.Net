using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyChannelNameParams
{
    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("channel_name")]
    public required string ChannelName { get; set; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; set; }
}
