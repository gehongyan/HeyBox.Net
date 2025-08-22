using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class CurrentUserPermission
{
    [JsonPropertyName("allow")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong Allow { get; set; }

    [JsonPropertyName("deny")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong Deny { get; set; }
}