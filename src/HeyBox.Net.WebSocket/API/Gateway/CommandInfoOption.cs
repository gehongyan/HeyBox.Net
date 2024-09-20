using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class CommandInfoOption
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required SlashCommandOptionType Type { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}