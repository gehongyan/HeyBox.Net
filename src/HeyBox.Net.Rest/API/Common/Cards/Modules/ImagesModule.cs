using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class ImagesModule : ModuleBase
{
    [JsonPropertyName("urls")]
    public required ImageNode[] Urls { get; set; }
}
