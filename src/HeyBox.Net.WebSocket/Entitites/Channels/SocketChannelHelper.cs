namespace HeyBox.WebSocket;

internal static class SocketChannelHelper
{
    /// <exception cref="NotSupportedException">Unexpected <see cref="ISocketMessageChannel"/> type.</exception>
    public static void AddMessage(ISocketMessageChannel channel, HeyBoxSocketClient heyBox, SocketMessage msg)
    {
        switch (channel)
        {
            case SocketDMChannel dmChannel:
                dmChannel.AddMessage(msg);
                break;
            case SocketTextChannel textChannel:
                textChannel.AddMessage(msg);
                break;
            default:
                throw new NotSupportedException($"Unexpected {nameof(ISocketMessageChannel)} type.");
        }
    }

    public static IReadOnlyCollection<SocketMessage> GetCachedMessages(
        ISocketMessageChannel channel, HeyBoxSocketClient heyBox, MessageCache? messages,
        ulong? referenceMessageId, Direction dir, int limit) =>
        messages?.GetMany(referenceMessageId, dir, limit) ?? [];
}
