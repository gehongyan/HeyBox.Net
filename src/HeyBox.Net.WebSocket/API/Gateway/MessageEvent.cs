using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API.Gateway;

internal class MessageEvent
{
    [JsonPropertyName("addition")]
    public required string Addition { get; set; }

    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("channel_name")]
    public required string ChannelName { get; set; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; set; }

    [JsonPropertyName("im_seq")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong ImSeq { get; set; }

    [JsonPropertyName("img")]
    public required string Img { get; set; }

    [JsonPropertyName("img_info")]
    public required MessageImageInfo ImgInfo { get; set; }

    [JsonPropertyName("is_resource_chan")]
    public required bool IsResourceChan { get; set; }

    [JsonPropertyName("level")]
    public required int Level { get; set; }

    [JsonPropertyName("msg")]
    public required string Msg { get; set; }

    [JsonPropertyName("msg_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong MsgId { get; set; }

    [JsonPropertyName("msg_type")]
    public required MessageType MsgType { get; set; }

    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("on_risk")]
    public required bool OnRisk { get; set; }

    [JsonPropertyName("online_desc")]
    public required string OnlineDesc { get; set; }

    [JsonPropertyName("online_state")]
    public required int OnlineState { get; set; }

    [JsonPropertyName("receive_type")]
    public required int ReceiveType { get; set; }

    [JsonPropertyName("roles")]
    public required string[] Roles { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("root_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RootId { get; set; }

    [JsonPropertyName("send_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public required DateTimeOffset SendTime { get; set; }

    [JsonPropertyName("state")]
    public required int State { get; set; }

    [JsonPropertyName("user_id")]
    public required int UserId { get; set; }

    [JsonPropertyName("user_info")]
    public required MessageUserInfo UserInfo { get; set; }
}

internal class MessageImageInfo
{
    [JsonPropertyName("fsize")]
    public required int FileSize { get; set; }

    [JsonPropertyName("height")]
    public required int Height { get; set; }

    [JsonPropertyName("mimetype")]
    public required string MimeType { get; set; }

    [JsonPropertyName("original_url")]
    public required string OriginalUrl { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("width")]
    public required int Width { get; set; }
}

internal class MessageUserInfo
{
    [JsonPropertyName("continuous_days")]
    public required int ContinuousDays { get; set; }

    [JsonPropertyName("hide_fire_flag")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool HideFireFlag { get; set; }

    [JsonPropertyName("remarks")]
    public required string Remarks { get; set; }

    [JsonPropertyName("user_base_info")]
    public required MessageUserBaseInfo UserBaseInfo { get; set; }

    [JsonPropertyName("user_mute_info")]
    public required MessageUserMuteInfo UserMuteInfo { get; set; }
}

internal class MessageUserBaseInfo
{
    [JsonPropertyName("avatar")]
    public required string Avatar { get; set; }

    [JsonPropertyName("level")]
    public required int Level { get; set; }

    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required ulong[] Roles { get; set; }

    [JsonPropertyName("user_id")]
    public required uint UserId { get; set; }
}

internal class MessageUserMuteInfo
{
    [JsonPropertyName("ban")]
    public required JsonElement Ban { get; set; }

    [JsonPropertyName("earphone_state")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool EarphoneState { get; set; }

    [JsonPropertyName("mute")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool Mute { get; set; }

    [JsonPropertyName("mute_earphone")]
    public required bool MuteEarphone { get; set; }

    [JsonPropertyName("mute_room")]
    public required bool MuteRoom { get; set; }

    [JsonPropertyName("mute_state")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool MuteState { get; set; }
}
