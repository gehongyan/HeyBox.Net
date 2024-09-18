namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的频道。
/// </summary>
public class RestChannel : RestEntity<ulong>, IChannel
{
    #region RestChannel

    internal RestChannel(BaseHeyBoxClient client, ulong id)
        : base(client, id)
    {
    }

    internal static RestChannel Create(BaseHeyBoxClient client, ulong roomId, ChannelType type, ulong id) =>
        type switch
        {
            ChannelType.Text => RestRoomChannel.Create(client, new RestRoom(client, roomId), type, id),
            _ => new RestChannel(client, id)
        };

    #endregion
}