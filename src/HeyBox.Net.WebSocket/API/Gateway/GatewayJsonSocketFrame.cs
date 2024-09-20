using System.Text.Json;

namespace HeyBox.API.Gateway;

internal readonly struct GatewayJsonSocketFrame
(
    string type,
    ulong sequence,
    JsonElement data,
    DateTimeOffset timestamp) : IGatewaySocketFrame
{
    public GatewaySocketFrameFormat Format => GatewaySocketFrameFormat.Json;
    public string Type { get; } = type;
    public ulong Sequence { get; } = sequence;
    public JsonElement Data { get; } = data;
    public DateTimeOffset Timestamp { get; } = timestamp;
}
