using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomRolesResponse
{
    [JsonPropertyName("roles")]
    public required IReadOnlyCollection<Role> Roles { get; init; }
 }
