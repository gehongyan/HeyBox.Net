using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class UpdateChannelPermissionOverwritesParams
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("roles")]
    public required RolePermissionOverwrite[] Roles { get; set; }

    [JsonPropertyName("users")]
    public required UserPermissionOverwrite[] Users { get; set; }
}