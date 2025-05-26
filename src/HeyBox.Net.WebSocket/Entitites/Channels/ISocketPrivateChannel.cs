using HeyBox.WebSocket;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的私有频道，只有特定的用户可以访问。
/// </summary>
public interface ISocketPrivateChannel : IPrivateChannel
{
    /// <inheritdoc cref="HeyBox.IPrivateChannel.Recipients" />
    new IReadOnlyCollection<SocketUser> Recipients { get; }
}