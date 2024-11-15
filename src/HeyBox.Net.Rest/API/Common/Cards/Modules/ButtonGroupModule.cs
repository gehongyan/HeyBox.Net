using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class ButtonGroupModule : ModuleBase
{
    [JsonPropertyName("btns")]
    public required ButtonNode[] Buttons { get; set; }
}
