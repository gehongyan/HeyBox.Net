using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomIntegration
{
    [JsonPropertyName("used_bot_commands")]
    public object? UsedBotCommands { get; set; } // TODO
}