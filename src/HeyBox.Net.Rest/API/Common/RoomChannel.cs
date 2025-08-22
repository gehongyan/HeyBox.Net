using System.Text.Json.Serialization;
using HeyBox.API.Rest;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class RoomChannel
{
    [JsonPropertyName("api_type")]
    [JsonConverter(typeof(VoiceChannelSdkProviderJsonConverter))]
    public required VoiceChannelSdkProvider ApiType { get; set; }

    [JsonPropertyName("max_count")]
    public int MaxCount { get; set; }

    [JsonPropertyName("follow_group")]
    public int FollowGroup { get; set; }

    [JsonPropertyName("follow_group_v1")]
    public int FollowGroupV1 { get; set; }

    [JsonPropertyName("audio_bitrate")]
    public int AudioBitrate { get; set; }

    [JsonPropertyName("tip_sound_user_limit")]
    public int TipSoundUserLimit { get; set; }

    [JsonPropertyName("is_private")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool IsPrivate { get; set; }

    [JsonPropertyName("chan_member_limit")]
    public int ChanMemberLimit { get; set; }

    [JsonPropertyName("parent_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong? ParentId { get; set; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("channel_name")]
    public required string ChannelName { get; set; }

    [JsonPropertyName("channel_type")]
    public ChannelType ChannelType { get; set; }

    [JsonPropertyName("channel_create_by")]
    public uint ChannelCreateBy { get; set; }

    [JsonPropertyName("create_by")]
    public uint CreateBy { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("state")]
    public int State { get; set; } // TODO

    [JsonPropertyName("password")]
    public bool Password { get; set; }

    [JsonPropertyName("channel_list")]
    public required RoomChannel[] ChannelList { get; set; }

    [JsonPropertyName("permission_overwrites")]
    public required RolePermissionOverwrite[] PermissionOverwrites { get; set; }

    [JsonPropertyName("permission_sync")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool PermissionSync { get; set; }

    [JsonPropertyName("user_perm")]
    public required CurrentUserPermission UserPerm { get; set; }
}
