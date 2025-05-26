using System.Diagnostics;
using System.Drawing;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的私聊频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestDMChannel : RestChannel, IDMChannel, IRestPrivateChannel, IRestMessageChannel
{
    /// <inheritdoc cref="HeyBox.IDMChannel.Id" />
    public new uint Id { get; }

    /// <summary>
    ///     获取参与到此私聊频道中的当前用户。
    /// </summary>
    public RestUser CurrentUser { get; }

    /// <inheritdoc cref="HeyBox.IDMChannel.Recipient" />
    public RestUser Recipient { get; }

    /// <summary>
    ///     获取参与到此私聊频道中的所有用户。
    /// </summary>
    public IReadOnlyCollection<RestUser> Users => [CurrentUser, Recipient];

    internal RestDMChannel(BaseHeyBoxClient client, uint recipientId)
        : base(client, recipientId)
    {
        Id = recipientId;
        Recipient = new RestUser(Client, recipientId);
        if (client.CurrentUser is RestUser restUser)
            CurrentUser = restUser;
        else if (client.CurrentUser is not null)
            CurrentUser = new RestUser(Client, client.CurrentUser.Id);
        else
            throw new InvalidOperationException("The current user is not set well via login.");
    }

    internal RestDMChannel(BaseHeyBoxClient client, RestUser recipient)
        : base(client, recipient.Id)
    {
        Recipient = recipient;
        if (client.CurrentUser is RestUser restUser)
            CurrentUser = restUser;
        else if (client.CurrentUser is not null)
            CurrentUser = new RestUser(Client, client.CurrentUser.Id);
        else
            throw new InvalidOperationException("The current user is not set well via login.");
    }

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

    #region IRestPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<RestUser> IRestPrivateChannel.Recipients => [Recipient];

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
