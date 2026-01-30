using System.Drawing;
using HeyBox.Rest;

namespace HeyBox.WebSocket;
using Model = API.Gateway.ChannelBaseInfo;

/// <summary>
///     表示房间中一个基于网关的具有文字聊天能力的频道，可以发送和接收消息。
/// </summary>
public class SocketTextChannel : SocketRoomChannel, ITextChannel, ISocketMessageChannel
{
    private readonly MessageCache? _messages;

    /// <inheritdoc />
    public bool IsPrivate { get; private set; }

    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public bool IsPermissionSynced { get; private set; }

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionChannel(Id);

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => _messages?.Messages ?? [];

    internal SocketTextChannel(HeyBoxSocketClient client, ulong id, SocketRoom room)
        : base(client, id, room)
    {
        Type = ChannelType.Text;
        if (Client.MessageCacheSize > 0)
            _messages = new MessageCache(Client);
    }

    internal static new SocketTextChannel Create(SocketRoom room, ClientState state, Model model)
    {
        SocketTextChannel entity = new(room.Client, model.ChannelId, room);
        entity.Update(state, model);
        return entity;
    }

    internal void AddMessage(SocketMessage msg) => _messages?.Add(msg);

    internal SocketMessage? RemoveMessage(ulong id) => _messages?.Remove(id);

    #region Messages

    /// <inheritdoc />
    public SocketMessage? GetCachedMessage(ulong id) => _messages?.Get(id);

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> GetCachedMessages(int limit = HeyBoxConfig.MaxMessagesPerBatch) =>
        SocketChannelHelper.GetCachedMessages(this, Client, _messages, null, Direction.Before, limit);

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> GetCachedMessages(ulong referenceMessageId,
        Direction dir, int limit = HeyBoxConfig.MaxMessagesPerBatch) =>
        SocketChannelHelper.GetCachedMessages(this, Client, _messages, referenceMessageId, dir, limit);

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> GetCachedMessages(IMessage referenceMessage,
        Direction dir, int limit = HeyBoxConfig.MaxMessagesPerBatch) =>
        SocketChannelHelper.GetCachedMessages(this, Client, _messages, referenceMessage.Id, dir, limit);


    #endregion

    #region Users

    /// <inheritdoc />
    public override SocketRoomUser? GetUser(ulong id)
    {
        if (Room.GetUser(id) is not { } user) return null;
        ulong roomPerms = Permissions.ResolveRoom(Room, user);
        ulong channelPerms = Permissions.ResolveChannel(Room, user, this, roomPerms);
        return Permissions.GetValue(channelPerms, ChannelPermission.ViewChannel) ? user : null;
    }

    #endregion

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(System.String,System.String,HeyBox.AttachmentType,System.Nullable{System.Drawing.Size},IMessageReference,HeyBox.RequestOptions)"/>
    public Task<IUserMessage> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null,
        RequestOptions? options = null)
    {
        string name = filename ?? Path.GetFileName(path);
        return ChannelHelper.SendFileAsync(this, Client, path, name, type, imageSize, messageReference, options);
    }

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(System.IO.Stream,System.String,HeyBox.AttachmentType,System.Nullable{System.Drawing.Size},IMessageReference,HeyBox.RequestOptions)"/>
    public Task<IUserMessage> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null,
        RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, stream, filename, type, imageSize, messageReference, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(HeyBox.FileAttachment,IMessageReference,HeyBox.RequestOptions)"/>
    public Task<IUserMessage> SendFileAsync(FileAttachment attachment,
        IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, attachment, messageReference, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendTextAsync(System.String,System.Collections.Generic.IEnumerable{HeyBox.FileAttachment},IMessageReference,HeyBox.RequestOptions)"/>
    public Task<IUserMessage> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendTextAsync(this, Client, text, imageFileInfos, messageReference, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendCardAsync(ICard card,
        IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendCardAsync(this, Client, card, messageReference, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendCardsAsync(IEnumerable<ICard> cards,
        IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendCardsAsync(this, Client, cards, messageReference, options);

    /// <inheritdoc />
    public Task DeleteMessageAsync(IMessage message, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(message, Client, options);

    /// <inheritdoc />
    public Task DeleteMessageAsync(ulong messageId, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(messageId, this, Client, options);
}
