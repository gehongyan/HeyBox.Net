using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class BotAccountInfo
{
    [JsonPropertyName("avatar_decoration")]
    public AvatarDecoration? AvatarDecoration { get; set; }

    [JsonPropertyName("tag")]
    public AccountTag? Tag { get; set; }

    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; } // -1 表示内置命令

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("bot")]
    public bool Bot { get; set; }

    [JsonPropertyName("roles")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong[]? Roles { get; set; }
}
