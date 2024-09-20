using System.Text.Json.Serialization;

namespace HeyBox.API.Gateway;

internal class ChannelBaseInfo
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("channel_name")]
    public required string ChannelName { get; set; }

    [JsonPropertyName("channel_type")]
    public required ChannelType ChannelType { get; set; }
}