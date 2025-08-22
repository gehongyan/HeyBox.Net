using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomGameTheme
{
    [JsonPropertyName("theme_role_map")]
    public required Dictionary<ulong, string> ThemeRoleMap { get; set; }

    [JsonPropertyName("platform")]
    public required string Platform { get; set; }

    [JsonPropertyName("theme_id")]
    public int ThemeId { get; set; }

    [JsonPropertyName("sync_show_game_name_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset SyncShowGameNameTime { get; set; }

    [JsonPropertyName("update_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset UpdateTime { get; set; }

    [JsonPropertyName("sync_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset SyncTime { get; set; }

    [JsonPropertyName("weekly_update_time")]
    public int WeeklyUpdateTime { get; set; }

    [JsonPropertyName("show_game_name")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowGameName { get; set; }

    [JsonPropertyName("show_game_card")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowGameCard { get; set; }

    [JsonPropertyName("group_by_role")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool GroupByRole { get; set; }
}