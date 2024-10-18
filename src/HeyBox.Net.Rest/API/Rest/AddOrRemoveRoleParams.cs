using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class AddOrRemoveRoleParams
{
    [JsonPropertyName("to_user_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong UserId { get; init; }

    [JsonPropertyName("role_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoleId { get; init; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }
}
