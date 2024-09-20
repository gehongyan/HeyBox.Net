namespace HeyBox.WebSocket;
using Model = API.Gateway.CommandInfo;

/// <summary>
///     表示一个通过网关接收的交互。
/// </summary>
public abstract class SocketInteraction : SocketEntity<ulong>, IHeyBoxInteraction
{
    /// <summary>
    ///     获取执行此交互所在的频道。
    /// </summary>
    public ISocketMessageChannel? Channel { get; private set; }

    /// <inheritdoc />
    public ulong? ChannelId { get; private set; }

    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.User" />
    public SocketUser User { get; private set; }

    /// <inheritdoc />
    public InteractionType Type { get; private set; }

    /// <inheritdoc />
    public IHeyBoxInteractionData Data { get; protected set; }

    /// <inheritdoc />
    public ulong? RoomId { get; private set; }

    internal SocketInteraction(HeyBoxSocketClient client, ulong id, SocketTextChannel channel, SocketRoomUser user)
        : base(client, id)
    {
        Channel = channel;
        ChannelId = channel.Id;
        RoomId = channel.Room.Id;
        User = user;
        Data = SocketInteractionData.Instance;
    }

    internal static SocketInteraction Create(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user)
    {
        if (model.Type == ApplicationCommandType.SlashCommand)
            return SocketSlashCommand.Create(client, model, channel, user);

        throw new InvalidOperationException("Unknown interaction type.");
    }

    #region IHeyBoxInteraction

    /// <inheritdoc/>
    IUser IHeyBoxInteraction.User => User;

    #endregion
}
