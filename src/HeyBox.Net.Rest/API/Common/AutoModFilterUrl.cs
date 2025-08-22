using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class AutoModFilterUrl : AutoModFilterWord
{
    [JsonPropertyName("only_send_chat_url")]
    public bool OnlySendChatUrl { get; set; }
}