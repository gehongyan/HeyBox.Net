using System.Collections.Immutable;
using HeyBox.API.Rest;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的通用消息。
/// </summary>
public abstract class RestMessage : RestEntity<ulong>, IMessage
{
    private ImmutableArray<ITag> _tags = [];

    /// <inheritdoc/>
    public MessageType Type { get; }

    /// <inheritdoc />
    public MessageSource Source { get; }

    /// <inheritdoc />
    public IMessageChannel Channel { get; }

    /// <inheritdoc />
    public IUser Author { get; }

    /// <inheritdoc />
    public string Content { get; internal set; } = string.Empty;

    /// <inheritdoc />
    public string CleanContent => MessageHelper.SanitizeMessage(this);

    /// <inheritdoc />
    public IMessageReference? Reference { get; internal set; }

    /// <inheritdoc />
    public IReadOnlyCollection<ITag> Tags => [];

    internal IReadOnlyCollection<FileAttachment>? ImageFileInfos { get; private set; }

    /// <inheritdoc />
    public virtual IReadOnlyCollection<ICard> Cards => [];

    /// <inheritdoc />
    protected RestMessage(BaseHeyBoxClient client, ulong id, MessageType messageType,
        IMessageChannel channel, IUser author, MessageSource source)
        : base(client, id)
    {
        Type = messageType;
        Channel = channel;
        Author = author;
        Source = source;
    }

    internal virtual void Update(SendChannelMessageParams args, SendChannelMessageResponse model)
    {
        Content = args.Message;
        Reference = args.ReplyId.HasValue ? new MessageReference(args.ReplyId.Value) : null;

        IRoom? room = (Channel as IRoomChannel)?.Room;
        _tags = MessageHelper.ParseTags(args.Message, Channel, room, []);
    }

    internal virtual void Update(SendUserMessageParams args, SendUserMessageResponse model)
    {
        Content = args.Message;
        _tags = MessageHelper.ParseTags(args.Message, Channel, null, []);
    }

    internal virtual void Update(IReadOnlyCollection<FileAttachment>? imageFileInfos)
    {
        ImageFileInfos = imageFileInfos;
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
