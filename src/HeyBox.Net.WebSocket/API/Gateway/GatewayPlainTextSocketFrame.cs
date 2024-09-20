namespace HeyBox.API.Gateway;

internal readonly struct GatewayPlainTextSocketFrame(string data) : IGatewaySocketFrame
{
    public GatewaySocketFrameFormat Format => GatewaySocketFrameFormat.PlainText;
    public string Data { get; } = data;
}
