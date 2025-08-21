using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class UserPermissionOverwrite
{
    [JsonPropertyName("to_user_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required uint ToUserId { get; set; }

    [JsonPropertyName("allow")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Allow { get; set; }

    [JsonPropertyName("deny")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Deny { get; set; }
}