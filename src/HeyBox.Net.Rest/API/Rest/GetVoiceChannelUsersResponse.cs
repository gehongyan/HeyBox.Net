using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetVoiceChannelUsersResponse
{
    [JsonPropertyName("user_ids")]
    public required uint[] UserIds { get; set; }
}
