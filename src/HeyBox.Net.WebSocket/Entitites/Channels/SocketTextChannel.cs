using System.Drawing;
using HeyBox.Rest;

namespace HeyBox.WebSocket;
using Model = API.Gateway.ChannelBaseInfo;

/// <summary>
///     表示房间中一个基于网关的具有文字聊天能力的频道，可以发送和接收消息。
/// </summary>
public class SocketTextChannel : SocketRoomChannel, ITextChannel, ISocketMessageChannel
{
    /// <inheritdoc />
    public string Mention => MentionUtils.MentionChannel(Id);

    internal SocketTextChannel(HeyBoxSocketClient client, ulong id, SocketRoom room)
        : base(client, id, room)
    {
        Type = ChannelType.Text;
    }

    internal static new SocketTextChannel Create(SocketRoom room, ClientState state, Model model)
    {
        SocketTextChannel entity = new(room.Client, model.ChannelId, room);
        entity.Update(state, model);
        return entity;
    }

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(System.String,System.String,HeyBox.AttachmentType,System.Nullable{System.Drawing.Size},HeyBox.IQuote,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IQuote? quote = null,
        RequestOptions? options = null)
    {
        string name = filename ?? Path.GetFileName(path);
        return ChannelHelper.SendFileAsync(this, Client, path, name, type, imageSize, quote, options);
    }

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(System.IO.Stream,System.String,HeyBox.AttachmentType,System.Nullable{System.Drawing.Size},HeyBox.IQuote,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IQuote? quote = null,
        RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, stream, filename, type, imageSize, quote, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendFileAsync(HeyBox.FileAttachment,HeyBox.IQuote,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendFileAsync(FileAttachment attachment,
        IQuote? quote = null, RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, attachment, quote, options);

    /// <inheritdoc cref="HeyBox.IMessageChannel.SendTextAsync(System.String,System.Collections.Generic.IEnumerable{HeyBox.FileAttachment},HeyBox.IQuote,HeyBox.RequestOptions)"/>
    public Task<Cacheable<IUserMessage, ulong>> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, IQuote? quote = null, RequestOptions? options = null) =>
        ChannelHelper.SendTextAsync(this, Client, text, imageFileInfos, quote, options);

}
