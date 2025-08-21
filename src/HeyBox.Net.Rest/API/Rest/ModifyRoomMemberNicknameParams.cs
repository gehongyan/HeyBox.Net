using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ModifyRoomMemberNicknameParams
{
    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("room_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required ulong RoomId { get; set; }

    [JsonPropertyName("to_user_id")]
    public required uint ToUserId { get; set; }
}
