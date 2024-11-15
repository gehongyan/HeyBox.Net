using HeyBox.Net.Converters;
using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class ModuleBase : IModule
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(ModuleTypeConverter))]
    public required ModuleType Type { get; set; }
}
