using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotSlashCommandChoice
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public SlashCommandOptionType Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}