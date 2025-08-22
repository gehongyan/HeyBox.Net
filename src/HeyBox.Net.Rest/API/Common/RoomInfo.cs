using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomInfo
{
    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("room")]
    public required ExtendedRoom Room { get; set; }

    [JsonPropertyName("channels")]
    public required RoomChannel[] Channels { get; set; }

    [JsonPropertyName("roles")]
    public required Role[] Roles { get; set; }

    [JsonPropertyName("my_roles")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong[] MyRoles { get; set; }

    [JsonPropertyName("my_room_info")]
    public required RoomCurrentUserInfo MyRoomInfo { get; set; }

    [JsonPropertyName("user_notify_settings")]
    public required IReadOnlyDictionary<ulong, UserNotifySetting> UserNotifySettings { get; set; }

    [JsonPropertyName("read_states")]
    public required IReadOnlyDictionary<ulong, ReadState> ReadStates { get; set; }

    [JsonPropertyName("bot_infos")]
    public required RoomBotInfo[] BotInfos { get; set; }

    [JsonPropertyName("integration")]
    public required RoomIntegration Integration { get; set; }
}