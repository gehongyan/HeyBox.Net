using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RolePermissionOverwrite
{
    [JsonPropertyName("role_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoleId { get; set; }

    [JsonPropertyName("allow")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong Allow { get; set; }

    [JsonPropertyName("deny")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong Deny { get; set; }
}
