using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomsPagedResponse : IPageInfoProvider
{
    [JsonPropertyName("rooms")]
    public required Room[] Rooms { get; set; }

    [JsonPropertyName("total")]
    public required int Total { get; set; }

    /// <inheritdoc />
    int IPageInfoProvider.TotalCount => Total;
}
