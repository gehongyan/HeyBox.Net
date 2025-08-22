using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomBotInfo
{
    [JsonPropertyName("bot_account_info")]
    public required BotAccountInfo BotAccountInfo { get; set; }

    [JsonPropertyName("commands")]
    public required BotSlashCommand[] Commands { get; set; }

    [JsonPropertyName("bot_bind_role")]
    public required Role BotBindRole { get; set; }

    [JsonPropertyName("creator")]
    public required BotCreator Creator { get; set; }

    [JsonPropertyName("permissions")]
    public required BotPermission[] Permissions { get; set; }

    [JsonPropertyName("joined_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset? JoinedTime { get; set; }

    [JsonPropertyName("display_setting")]
    public BotDisplaySetting? DisplaySetting { get; set; }
}
