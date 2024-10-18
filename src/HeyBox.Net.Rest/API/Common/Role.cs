using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class Role
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }

    [JsonPropertyName("color_list")]
    [JsonConverter(typeof(GradientColorConverter))]
    public GradientColor? GradientColor { get; set; }

    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("room_id")]
    public ulong RoomId { get; set; }

    [JsonPropertyName("permissions")]
    public ulong Permissions { get; set; }

    [JsonPropertyName("type")]
    public RoleType Type { get; set; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(ColorConverter))]
    public Color Color { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("del_tag")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool Deleted { get; set; }

    [JsonPropertyName("hoist")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool Hoist { get; set; }

    [JsonPropertyName("mentionable")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public bool Mentionable { get; set; }

    [JsonPropertyName("creator")]
    public ulong Creator { get; set; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = TimestampUnit.Seconds)]
    public DateTimeOffset? CreateTime { get; set; }
}
