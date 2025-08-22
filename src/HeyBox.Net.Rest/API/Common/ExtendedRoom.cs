using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class ExtendedRoom
{
    [JsonPropertyName("room_join_verify")]
    public required RoomJoinVerify RoomJoinVerify { get; set; }

    [JsonPropertyName("room_setting")]
    public required RoomSetting RoomSetting { get; set; }

    [JsonPropertyName("room_bind_team")]
    public required RoomBindTeam RoomBindTeam { get; set; }

    [JsonPropertyName("room_fire_flag")]
    public required RoomFireFlag RoomFireFlag { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("room_name")]
    public required string RoomName { get; set; }

    [JsonPropertyName("room_avatar")]
    public required string RoomAvatar { get; set; }

    [JsonPropertyName("room_pic")]
    public required string RoomPic { get; set; }

    [JsonPropertyName("public_id")]
    public required string PublicId { get; set; }

    [JsonPropertyName("introduction")]
    public required string Introduction { get; set; }

    [JsonPropertyName("main_color_v2")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool MainColorV2 { get; set; }

    [JsonPropertyName("bar_main_color")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool BarMainColor { get; set; }

    [JsonPropertyName("bar_main_color_v2")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool BarMainColorV2 { get; set; }

    [JsonPropertyName("bg_pic")]
    public required string BgPic { get; set; }

    [JsonPropertyName("bg_color")]
    [JsonConverter(typeof(BackgroundGradientColorConverter))]
    public GradientColor? BgColor { get; set; }

    [JsonPropertyName("bar_color")]
    [JsonConverter(typeof(BackgroundGradientColorConverter))]
    public GradientColor? BarColor { get; set; }

    [JsonPropertyName("bg_pic_main_color")]
    public required string BgPicMainColor { get; set; } // TODO

    [JsonPropertyName("blur_rate")]
    public int BlurRate { get; set; }

    [JsonPropertyName("transparency")]
    public int Transparency { get; set; }

    [JsonPropertyName("main_color")]
    [JsonConverter(typeof(NullableNumberColorJsonConverter))]
    public Color? MainColor { get; set; }

    [JsonPropertyName("show_channel_bar_filter")]
    public bool ShowChannelBarFilter { get; set; }

    [JsonPropertyName("can_change_bg_pic")]
    [BooleanJsonConverter(Format = BooleanFormat.Boolean)]
    public bool CanChangeBgPic { get; set; }

    [JsonPropertyName("show_room_level")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool ShowRoomLevel { get; set; }

    [JsonPropertyName("game_theme")]
    public RoomGameTheme? GameTheme { get; set; }

    [JsonPropertyName("room_notify_type")]
    public NotifyType RoomNotifyType { get; set; }

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

    [JsonPropertyName("create_by")]
    public uint CreateBy { get; set; }

    [JsonPropertyName("link_id")]
    public int LinkId { get; set; }

    [JsonPropertyName("is_public")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool IsPublic { get; set; }

    [JsonPropertyName("is_hot")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool IsHot { get; set; }

    [JsonPropertyName("room_type")]
    public int RoomType { get; set; } // TODO

    [JsonPropertyName("apply_hot_status")]
    public ApplyHotStatus ApplyHotStatus { get; set; }

    [JsonPropertyName("can_decorate")]
    public bool CanDecorate { get; set; }

    [JsonPropertyName("room_task")]
    public RoomTasks? RoomTask { get; set; }

    [JsonPropertyName("room_icon")]
    public RoomIcon[]? RoomIcon { get; set; }

    [JsonPropertyName("member_count")]
    public int MemberCount { get; set; }

    [JsonPropertyName("online_count")]
    public int OnlineCount { get; set; }

    [JsonPropertyName("audio_count")]
    public int AudioCount { get; set; }

    [JsonPropertyName("update_time")]
    public ulong UpdateTime { get; set; } // TODO

    [JsonPropertyName("enable_switch_line")]
    public bool EnableSwitchLine { get; set; }

    [JsonPropertyName("av_line_list")]
    public required RoomAvLineList[] AvLineList { get; set; }

    [JsonPropertyName("channel_fold_user")]
    public int ChannelFoldUser { get; set; }

    [JsonPropertyName("members")]
    public object? Members { get; set; } // TODO

    [JsonPropertyName("screen_share_members")]
    public object? ScreenShareMembers { get; set; } // TODO

    [JsonPropertyName("audio_share_members")]
    public object? AudioShareMembers { get; set; } // TODO

    [JsonPropertyName("promise_display_info")]
    public required string PromiseDisplayInfo { get; set; } // TODO

    [JsonPropertyName("bind_game_infos")]
    public RoomGameInfo[]? BindGameInfos { get; set; }

    [JsonPropertyName("customization")]
    public required RoomCustomization Customization { get; set; }

    [JsonPropertyName("enable_customization")]
    public bool EnableCustomization { get; set; }

    [JsonPropertyName("rec_desc")]
    public required string RecDesc { get; set; }

    [JsonPropertyName("anc")]
    public object? Anc { get; set; } // TODO

    [JsonPropertyName("cron_anc_count")]
    public int CronAncCount { get; set; }

    [JsonPropertyName("act")]
    public object[]? Act { get; set; } // TODO

    [JsonPropertyName("act_count")]
    public int ActCount { get; set; }

    [JsonPropertyName("active_user_count")]
    public int ActiveUserCount { get; set; }

    [JsonPropertyName("show_online_num")]
    public bool ShowOnlineNum { get; set; }

    [JsonPropertyName("show_room_active")]
    public bool ShowRoomActive { get; set; }
}
