using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomCustomizationGuidanceResource
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("img")]
    public required string Img { get; set; }
}
