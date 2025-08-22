using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomJoinVerify
{
    [JsonPropertyName("question")]
    public required string Question { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("verify_switch")]
    public bool VerifySwitch { get; set; }

    [JsonPropertyName("need_apply")]
    public bool NeedApply { get; set; }

    [JsonPropertyName("approvers")]
    public uint[]? Approvers { get; set; }

    [JsonPropertyName("is_must_need_text")]
    public bool IsMustNeedText { get; set; }

    [JsonPropertyName("is_must_need_img")]
    public bool IsMustNeedImg { get; set; }
}