using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotSlashCommand
{
    [JsonPropertyName("protocol")]
    public string? Protocol { get; set; } // TODO

    [JsonPropertyName("permission")]
    public string? Permission { get; set; } // TODO

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("options")]
    public required BotSlashCommandOption[] Options { get; set; }

    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong Id { get; set; }

    [JsonPropertyName("type")]
    public ApplicationCommandType Type { get; set; }
}