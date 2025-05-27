using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomsPagedResponse : PagedResponseBase
{
    [JsonPropertyName("rooms")]
    public required Room[] Rooms { get; set; }
}
