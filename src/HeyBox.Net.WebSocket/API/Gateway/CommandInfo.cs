using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class CommandInfo
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("options")]
    public CommandInfoOption[]? Options { get; set; }

    [JsonPropertyName("type")]
    public required ApplicationCommandType Type { get; set; }
}
