using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API.Rest;

internal class CreateRoomRoleParams
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    [JsonPropertyName("color_list")]
    [JsonConverter(typeof(GradientColorConverter))]
    public GradientColor? ColorList { get; init; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; init; }

    [JsonPropertyName("permissions")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong Permissions { get; init; }

    [JsonPropertyName("type")]
    public required RoleType Type { get; init; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(NullableColorConverter))]
    public Color? Color { get; init; }

    [JsonPropertyName("hoist")]
    [BooleanJsonConverter(Format = BooleanFormat.Number)]
    public required bool Hoist { get; init; }

    [JsonPropertyName("nonce")]
    public required string Nonce { get; init; }
}
