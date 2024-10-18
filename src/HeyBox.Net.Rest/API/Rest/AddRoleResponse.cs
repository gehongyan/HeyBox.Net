using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class AddRoleResponse
{
    [JsonPropertyName("user")]
    public required RoomUserRoleInfo User { get; init; }
}

internal class RoomUserRoleInfo
{
    [JsonPropertyName("role")]
    public required string[] Role { get; init; }

    [JsonPropertyName("user_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong UserId { get; init; }
}
