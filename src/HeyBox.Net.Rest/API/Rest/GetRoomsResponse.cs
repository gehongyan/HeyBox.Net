using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomsResponse
{
    [JsonPropertyName("rooms")]
    public required GetRoomsPagedResponse Rooms { get; set; }
}
