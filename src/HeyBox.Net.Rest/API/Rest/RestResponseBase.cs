using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.Rest.Converters;

namespace HeyBox.API.Rest;

internal class RestResponseBase
{
    [JsonPropertyName("status")]
    [JsonConverter(typeof(HeyBoxErrorCodeJsonConverter))]
    public required HeyBoxErrorCode Status { get; init; }

    [JsonPropertyName("msg")]
    public required string Message { get; init; }

    [JsonPropertyName("result")]
    public required JsonElement Result { get; init; }
}
