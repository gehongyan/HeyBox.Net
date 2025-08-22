using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class UserNotifySetting
{
    [JsonPropertyName("notify_type")]
    public NotifyType? NotifyType { get; set; }

    [JsonPropertyName("channel_notify_setting")]
    public NotifyType? ChannelNotifySetting { get; set; }
}