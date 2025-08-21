using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class KickOutFromChannelParams
{
    [JsonPropertyName("to_user_id")]
    public required uint ToUserId { get; set; }
}
