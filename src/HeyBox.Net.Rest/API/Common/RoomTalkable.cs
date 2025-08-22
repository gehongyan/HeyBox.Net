using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomTalkable
{
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }
}