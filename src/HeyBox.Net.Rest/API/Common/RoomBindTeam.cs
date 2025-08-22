using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomBindTeam
{
    [JsonPropertyName("appid")]
    public int Appid { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong RoomId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("room_sync_team")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool RoomSyncTeam { get; set; }

    [JsonPropertyName("team_sync_room")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool TeamSyncRoom { get; set; }

    [JsonPropertyName("game_name")]
    public required string GameName { get; set; }
}