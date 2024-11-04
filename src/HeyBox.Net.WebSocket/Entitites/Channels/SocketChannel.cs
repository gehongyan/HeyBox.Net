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

    /// <summary>
    ///     获取此频道中的一个用户。
    /// </summary>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 如果找到了具有指定 ID 的用户，则返回该用户；否则返回 <c>null</c>。 </returns>
    public SocketUser? GetUser(ulong id) => GetUserInternal(id);

    internal abstract SocketUser? GetUserInternal(ulong id);

    #endregion

    #region IChannel

    /// <inheritdoc />
    string IChannel.Name => string.Empty;

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(uint id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(null); //Overridden

    #endregion
}
