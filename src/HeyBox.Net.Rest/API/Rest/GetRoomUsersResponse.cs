using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomUsersResponse
{
    [JsonPropertyName("room_info")]
    public required GetRoomUsersPagedResponse RoomInfo { get; set; }
}
