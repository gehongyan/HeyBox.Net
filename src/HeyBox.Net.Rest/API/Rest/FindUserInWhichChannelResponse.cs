using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class FindUserInWhichChannelResponse
{
    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public ulong ChannelId { get; set; }
}
