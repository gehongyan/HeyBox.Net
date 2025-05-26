using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal abstract class PagedResponseBase
{
    [JsonPropertyName("total")]
    public required int Total { get; set; }

    [JsonPropertyName("offset")]
    public required int Offset { get; set; }

    [JsonPropertyName("limit")]
    public required int Limit { get; set; }
}
