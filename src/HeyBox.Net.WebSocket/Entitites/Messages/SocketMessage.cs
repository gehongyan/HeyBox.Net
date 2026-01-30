using System.Collections.Immutable;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过 WebSocket 接收的消息。
/// </summary>
public abstract class SocketMessage : SocketEntity<ulong>, IMessage
{
    private ImmutableArray<ITag> _tags = [];

    /// <inheritdoc/>
    public MessageType Type { get; private set; }

    /// <inheritdoc />
    public MessageSource Source { get; private set; }

    /// <inheritdoc />
    public IMessageChannel Channel { get; private set; }

    /// <inheritdoc />
    public IUser Author { get; private set; }

    /// <inheritdoc />
    public string Content { get; internal set; } = string.Empty;

    /// <inheritdoc />
    public string CleanContent => MessageHelper.SanitizeMessage(this);

    /// <inheritdoc />
    public DateTimeOffset Timestamp { get; private set; }

    /// <inheritdoc />
    public IMessageReference? Reference { get; private set; }

    /// <inheritdoc />
    public virtual IReadOnlyCollection<ITag> Tags => _tags;

    internal SocketMessage(HeyBoxSocketClient client, ulong id, MessageType messageType,
        ISocketMessageChannel channel, SocketUser author, MessageSource source)
        : base(client, id)
    {
        Type = messageType;
        Channel = channel;
        Author = author;
        Source = source;
    }

    internal virtual void Update(ClientState state, API.Gateway.MessageEvent model)
    {
        Content = model.Msg;
        Timestamp = model.SendTime;

        IRoom? room = (Channel as IRoomChannel)?.Room;
        _tags = MessageHelper.ParseTags(model.Msg, Channel, room, []);
    }

    #region Reactions

    /// <inheritdoc />
    /// <exception cref="NotSupportedException"> 此类型的消息不支持此操作。 </exception>
    public Task AddReactionAsync(IEmote emote, RequestOptions? options = null) =>
        Channel switch
        {
            ITextChannel textChannel => MessageHelper.AddReactionAsync(this, textChannel, emote, Client, options),
            _ => throw new NotSupportedException("The operation is not supported for this message type.")
        };

    /// <inheritdoc />
    /// <exception cref="NotSupportedException"> 此类型的消息不支持此操作。 </exception>
    public Task RemoveReactionAsync(IEmote emote, RequestOptions? options = null) =>
        Channel switch
        {
            ITextChannel textChannel => MessageHelper.RemoveReactionAsync(this, textChannel, emote, Client, options),
            _ => throw new NotSupportedException("The operation is not supported for this message type.")
        };

    #endregion
}
