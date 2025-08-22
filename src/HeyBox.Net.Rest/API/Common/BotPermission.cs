using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotPermission
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; } // TODO

    [JsonPropertyName("allow")]
    public bool Allow { get; set; }
}