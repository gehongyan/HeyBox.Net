using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetInviteCodeResponse
{
    [JsonPropertyName("invite_code")]
    public required string InviteCode { get; set; }
}
