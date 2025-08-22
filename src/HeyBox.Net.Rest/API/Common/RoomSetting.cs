using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomSetting
{
    [JsonPropertyName("show_room_level")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowRoomLevel { get; set; }

    [JsonPropertyName("game_theme")]
    public RoomGameTheme? GameTheme { get; set; }

    [JsonPropertyName("room_notify_type")]
    public NotifyType RoomNotifyType { get; set; }

    [JsonPropertyName("last_room_notify_type")]
    public NotifyType? LastRoomNotifyType { get; set; }

    [JsonPropertyName("room_default_channel")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? RoomDefaultChannel { get; set; }

    [JsonPropertyName("room_show_offline_user")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool RoomShowOfflineUser { get; set; }

    [JsonPropertyName("show_recommend_set_only_at")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowRecommendSetOnlyAt { get; set; }

    [JsonPropertyName("show_bind_game")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowBindGame { get; set; }

    [JsonPropertyName("welcome_channel")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? WelcomeChannel { get; set; }

    [JsonPropertyName("use_voice_pack_room_only")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool UseVoicePackRoomOnly { get; set; }

    [JsonPropertyName("show_apply_hot")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowApplyHot { get; set; }

    [JsonPropertyName("room_icon_id")]
    public int[]? RoomIconId { get; set; }

    [JsonPropertyName("bind_game_ids")]
    public int[]? BindGameIds { get; set; }

    [JsonPropertyName("similar_game_ids")]
    public int[]? SimilarGameIds { get; set; }

    [JsonPropertyName("similar_game_infos")]
    public RoomGameInfo[]? SimilarGameInfos { get; set; }

    [JsonPropertyName("bind_game_infos")]
    public RoomGameInfo[]? BindGameInfos { get; set; }

    [JsonPropertyName("auto_mod")]
    public required RoomAutoMod AutoMod { get; set; }

    [JsonPropertyName("talkable")]
    public RoomTalkable? Talkable { get; set; }

    [JsonPropertyName("default_permissions")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong DefaultPermissions { get; set; }

    [JsonPropertyName("hide_room_fire_flag")]
    public bool HideRoomFireFlag { get; set; }

    [JsonPropertyName("hide_member_fire_flag")]
    public bool HideMemberFireFlag { get; set; }

    [JsonPropertyName("block_threshold")]
    public RoomBanThreshold? BlockThreshold { get; set; }

    [JsonPropertyName("feedback_channel")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? FeedbackChannel { get; set; }

    [JsonPropertyName("matching_channel")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? MatchingChannel { get; set; }

    [JsonPropertyName("allow_auto_reply")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool AllowAutoReply { get; set; }

    [JsonPropertyName("allow_check_in")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool AllowCheckIn { get; set; }

    [JsonPropertyName("allow_exchange")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool AllowExchange { get; set; }

    [JsonPropertyName("checkin_channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? CheckinChannelId { get; set; }

    [JsonPropertyName("exchange_channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? ExchangeChannelId { get; set; }

    [JsonPropertyName("is_big")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool IsBig { get; set; }

    [JsonPropertyName("default_collapse_all_channel")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool DefaultCollapseAllChannel { get; set; }

    [JsonPropertyName("msg_ttl")]
    public int? MsgTtl { get; set; }

    [JsonPropertyName("allow_relation")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool AllowRelation { get; set; } // TODO
}
