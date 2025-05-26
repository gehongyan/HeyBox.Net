using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class Room
{
    [JsonPropertyName("room_id")]
    public required string RoomId { get; set; }

    [JsonPropertyName("room_name")]
    public required string RoomName { get; set; }

    [JsonPropertyName("room_avatar")]
    public required string RoomAvatar { get; set; }

    [JsonPropertyName("create_by")]
    public required int CreateBy { get; set; }

    [JsonPropertyName("room_pic")]
    public required string RoomPic { get; set; }

    [JsonPropertyName("is_public")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool IsPublic { get; set; }

    [JsonPropertyName("public_id")]
    public required string PublicId { get; set; }

    [JsonPropertyName("is_hot")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool IsHot { get; set; }

    [JsonPropertyName("join_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Milliseconds)]
    public required DateTimeOffset JoinTime { get; set; }
}
