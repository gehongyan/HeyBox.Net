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
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null,
        RequestOptions? options = null)
    {
        string name = filename ?? Path.GetFileName(path);
        return ChannelHelper.SendFileAsync(this, Client, path, name, type, imageSize, messageReference, options);
    }

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(System.IO.Stream,System.String,HeyBox.AttachmentType,System.Nullable{System.Drawing.Size},IMessageReference,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null,
        RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, stream, filename, type, imageSize, messageReference, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(HeyBox.FileAttachment,IMessageReference,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(FileAttachment attachment,
        IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, attachment, messageReference, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendTextAsync(System.String,System.Collections.Generic.IEnumerable{HeyBox.FileAttachment},IMessageReference,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, IMessageReference? messageReference = null, RequestOptions? options = null) =>
        ChannelHelper.SendTextAsync(this, Client, text, imageFileInfos, messageReference, options);

    #endregion
}
