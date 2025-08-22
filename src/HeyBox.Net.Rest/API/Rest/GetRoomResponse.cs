using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomResponse
{
    [JsonPropertyName("room_info")]
    public required RoomInfo RoomInfo { get; set; }

    [JsonPropertyName("screen_share_v2")]
    public bool ScreenShareV2 { get; set; }
}
