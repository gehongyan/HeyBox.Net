using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class Card : CardBase
{
    [JsonPropertyName("modules")]
    public required ModuleBase[] Modules { get; set; }
}
