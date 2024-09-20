namespace HeyBox;

internal class SocketInteractionData : IHeyBoxInteractionData
{
    public static SocketInteractionData Instance { get; } = new();

    private SocketInteractionData() { }
}
