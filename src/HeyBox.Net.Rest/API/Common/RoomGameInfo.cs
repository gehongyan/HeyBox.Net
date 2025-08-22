using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomGameInfo
{
    [JsonPropertyName("pic_url")]
    public required string PicUrl { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("appid")]
    public required int AppId { get; set; }
}