using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class RoomMemberJoinLeftEvent
{
    [JsonPropertyName("room_base_info")]
    public required RoomBaseInfo RoomBaseInfo { get; set; }

    [JsonPropertyName("state")]
    public required RoomMemberAction State { get; set; }

    [JsonPropertyName("user_info")]
    public required RoomUser UserInfo { get; set; }
}

internal enum RoomMemberAction
{
    Left = 0,
    Join = 1,
}
