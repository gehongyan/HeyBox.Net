using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomMute
{
    [JsonPropertyName("operator_id")]
    public uint? OperatorId { get; set; }

    [JsonPropertyName("mute")]
    public bool Mute { get; set; }
}