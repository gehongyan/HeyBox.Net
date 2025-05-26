using System.Diagnostics;
using System.Drawing;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于 SOCKET 的私聊频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketDMChannel : SocketChannel, IDMChannel, ISocketPrivateChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="HeyBox.IDMChannel.Id" />
    public new uint Id { get; }

    /// <summary>
    ///     获取参与到此私聊频道中的当前用户。
    /// </summary>
    public SocketUser CurrentUser { get; }

    /// <inheritdoc cref="HeyBox.IDMChannel.Recipient" />
    public SocketUser Recipient { get; }

    /// <summary>
    ///     获取参与到此私聊频道中的所有用户。
    /// </summary>
    public IReadOnlyCollection<SocketUser> Users => [CurrentUser, Recipient];

    internal SocketDMChannel(HeyBoxSocketClient client, SocketUser recipient)
        : base(client, recipient.Id)
    {
        Id = recipient.Id;
        Recipient = recipient;
        CurrentUser = client.CurrentUser
            ?? throw new InvalidOperationException("The current user is not set well via login.");
    }

    #region Users

    /// <inheritdoc cref="HeyBox.WebSocket.SocketChannel.GetUser(System.UInt64)" />
    public new SocketUser? GetUser(ulong id)
    {
        if (id == Recipient.Id) return Recipient;
        return id == Client.CurrentUser?.Id ? Client.CurrentUser : null;
    }

    /// <inheritdoc />
    internal override SocketUser? GetUserInternal(ulong id) => GetUser(id);

    #endregion

    /// <inheritdoc />
    public Task<IUserMessage> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, RequestOptions? options = null)
    {
        string name = filename ?? Path.GetFileName(path);
        return ChannelHelper.SendFileAsync(this, Client, path, name, type, imageSize, options);
    }

    /// <inheritdoc />
    public Task<IUserMessage> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, stream, filename, type, imageSize, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendFileAsync(FileAttachment attachment, RequestOptions? options = null) =>
        ChannelHelper.SendFileAsync(this, Client, attachment, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, RequestOptions? options = null) =>
        ChannelHelper.SendTextAsync(this, Client, text, imageFileInfos, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendCardAsync(ICard card, RequestOptions? options = null) =>
        ChannelHelper.SendCardAsync(this, Client, card, options);

    /// <inheritdoc />
    public Task<IUserMessage> SendCardsAsync(IEnumerable<ICard> cards, RequestOptions? options = null) =>
        ChannelHelper.SendCardsAsync(this, Client, cards, options);

    /// <summary>
    ///     获取此参与到此私聊频道的另外一位用户的包含 <c>@</c> 前缀的用户名及识别号格式化字符串。
    /// </summary>
    /// <returns> 一个表示此私聊频道的格式化字符串。 </returns>
    public override string ToString() => $"@{Recipient}";

    private string DebuggerDisplay => $"@{Recipient} ({Id}, DM)";

    #region IDMChannel

    /// <inheritdoc />
    IUser IDMChannel.Recipient => Recipient;

    #endregion

    #region ISocketPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<SocketUser> ISocketPrivateChannel.Recipients => [Recipient];

    #endregion

    #region IPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<IUser> IPrivateChannel.Recipients => [Recipient];

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendFileAsync(string path, string? filename,
        AttachmentType type, Size? imageSize, IMessageReference? messageReference, RequestOptions? options) =>
        SendFileAsync(path, filename, type, imageSize, options);

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendFileAsync(Stream stream, string filename,
        AttachmentType type, Size? imageSize, IMessageReference? messageReference, RequestOptions? options) =>
        SendFileAsync(stream, filename, type, imageSize, options);

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendFileAsync(FileAttachment attachment,
        IMessageReference? messageReference, RequestOptions? options) =>
        SendFileAsync(attachment, options);

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendTextAsync(string text, IEnumerable<FileAttachment>? imageFileInfos,
        IMessageReference? messageReference, RequestOptions? options) =>
        SendTextAsync(text, imageFileInfos, options);

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendCardAsync(ICard card,
        IMessageReference? messageReference, RequestOptions? options) =>
        SendCardAsync(card, options);

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendCardsAsync(IEnumerable<ICard> cards,
        IMessageReference? messageReference, RequestOptions? options) =>
        SendCardsAsync(cards, options);

    /// <inheritdoc />
    Task IMessageChannel.DeleteMessageAsync(IMessage message, RequestOptions? options) =>
        throw new NotSupportedException("Deleting message is not supported in an IDMChannel.");

    /// <inheritdoc />
    Task IMessageChannel.DeleteMessageAsync(ulong messageId, RequestOptions? options) =>
        throw new NotSupportedException("Deleting message is not supported in an IDMChannel.");

    #endregion
}
