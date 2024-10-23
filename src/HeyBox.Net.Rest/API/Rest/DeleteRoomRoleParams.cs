using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class DeleteRoomRoleParams
{
    [JsonPropertyName("role_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoleId { get; init; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }
}
