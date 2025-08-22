using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomBan
{
    [JsonPropertyName("operator_id")]
    public uint? OperatorId { get; set; }

    [JsonPropertyName("reason")]
    public required string Reason { get; set; }

    [JsonPropertyName("until")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong Until { get; set; } // TODO

    [JsonPropertyName("notify")]
    public bool Notify { get; set; }
}