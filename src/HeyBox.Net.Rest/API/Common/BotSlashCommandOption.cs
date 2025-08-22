using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotSlashCommandOption
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public SlashCommandOptionType Type { get; set; }

    [JsonPropertyName("choices")]
    public BotSlashCommandChoice[]? Choices { get; set; }
}
