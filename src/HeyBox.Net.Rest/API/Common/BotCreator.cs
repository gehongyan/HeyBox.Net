using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotCreator
{
    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("user_id")]
    public uint UserId { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("roles")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong[]? Roles { get; set; }
}