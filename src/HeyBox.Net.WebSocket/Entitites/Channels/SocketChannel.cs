namespace HeyBox.WebSocket;
using Model = API.Gateway.ChannelBaseInfo;

/// <summary>
///     表示一个基于网关的频道。
/// </summary>
public abstract class SocketChannel : SocketEntity<ulong>, IChannel
{
    #region SocketChannel

    internal SocketChannel(HeyBoxSocketClient client, ulong id)
        : base(client, id)
    {
    }

    internal abstract void Update(ClientState state, Model model);

    #endregion

    #region IChannel

    /// <inheritdoc />
    string IChannel.Name => string.Empty;

    #endregion
}
