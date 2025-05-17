using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomUser
{
    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("avatar_decoration")]
    public AvatarDecoration? AvatarDecoration { get; set; }

    [JsonPropertyName("bot")]
    public bool Bot { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("medals")]
    public JsonElement Medals { get; set; }

    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("roles")]
    public ulong[]? Roles { get; set; }

    [JsonPropertyName("room_nickname")]
    public string? RoomNickname { get; set; }

    [JsonPropertyName("tag")]
    public JsonElement Tag { get; set; }

    [JsonPropertyName("user_id")]
    public uint UserId { get; set; }
}
