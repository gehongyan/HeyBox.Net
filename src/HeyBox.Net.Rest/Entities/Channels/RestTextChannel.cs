using System.Drawing;

namespace HeyBox.Rest;

/// <summary>
///     表示房间中一个基于 REST 的具有文字聊天能力的频道，可以发送和接收消息。
/// </summary>
public class RestTextChannel : RestRoomChannel, IRestMessageChannel, ITextChannel
{
    #region RestTextChannel

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionChannel(Id);

    internal RestTextChannel(BaseHeyBoxClient client, IRoom room, ulong id)
        : base(client, room, id)
    {
        Type = ChannelType.Text;
    }

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

    #endregion
}
