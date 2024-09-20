using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class SenderInfo
{
    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("avatar_decoration")]
    public required AvatarDecoration AvatarDecoration { get; set; }

    [JsonPropertyName("bot")]
    public required bool Bot { get; set; }

    [JsonPropertyName("level")]
    public required int Level { get; set; }

    [JsonPropertyName("medals")]
    public required JsonElement Medals { get; set; }

    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required ulong[] Roles { get; set; }

    [JsonPropertyName("room_nickname")]
    public required string RoomNickname { get; set; }

    [JsonPropertyName("tag")]
    public required JsonElement Tag { get; set; }

    [JsonPropertyName("user_id")]
    public required uint UserId { get; set; }
}