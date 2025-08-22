using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomCurrentUserInfo
{
    [JsonPropertyName("room_nickname")]
    public required string RoomNickname { get; set; }

    [JsonPropertyName("show_room_bg")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowRoomBg { get; set; }

    [JsonPropertyName("ban")]
    public required RoomBan Ban { get; set; }

    [JsonPropertyName("mute_earphone")]
    public required RoomMute MuteEarphone { get; set; }

    [JsonPropertyName("mute_room")]
    public required RoomMute MuteRoom { get; set; }

    [JsonPropertyName("show_game_name")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowGameName { get; set; }

    [JsonPropertyName("show_game_card")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowGameCard { get; set; }

    [JsonPropertyName("read_anc_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset? ReadAncTime { get; set; }

    [JsonPropertyName("is_visitor")]
    public bool IsVisitor { get; set; }

    [JsonPropertyName("customization")]
    public required RoomCurrentUserCustomization Customization { get; set; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public DateTimeOffset? CreateTime { get; set; }

    [JsonPropertyName("continuous_days")]
    public int ContinuousDays { get; set; }

    [JsonPropertyName("hide_fire_flag")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool HideFireFlag { get; set; }

    [JsonPropertyName("show_fire_tip_time")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowFireTipTime { get; set; }

    [JsonPropertyName("room_act_unread_count")]
    public int RoomActUnreadCount { get; set; }

    [JsonPropertyName("last_close_room_act_id")]
    public required string LastCloseRoomActId { get; set; }
}
