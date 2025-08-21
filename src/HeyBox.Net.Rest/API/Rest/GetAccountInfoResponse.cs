using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetAccountInfoResponse
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
}
