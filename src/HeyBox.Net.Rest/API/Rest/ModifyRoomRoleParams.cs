using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyRoomRoleParams : CreateRoomRoleParams
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Id { get; init; }
}
