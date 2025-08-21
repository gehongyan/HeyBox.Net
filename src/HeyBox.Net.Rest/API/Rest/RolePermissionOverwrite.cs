using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class RolePermissionOverwrite
{
    [JsonPropertyName("role_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoleId { get; set; }

    [JsonPropertyName("allow")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Allow { get; set; }

    [JsonPropertyName("deny")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Deny { get; set; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; set; }
}