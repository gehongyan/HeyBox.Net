using System.Drawing;

namespace HeyBox;

/// <summary>
///     表示一个通用的私聊频道。
/// </summary>
public interface IDMChannel : IMessageChannel, IPrivateChannel, IEntity<uint>
{
    /// <summary>
    ///     获取此私聊频道的唯一标识符。
    /// </summary>
    new uint Id { get; }

    /// <summary>
    ///     获取参与到此私聊频道的另外一位用户。
    /// </summary>
    IUser Recipient { get; }

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="path"> 文件的路径。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="stream"> 文件的流。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="attachment"> 文件的附件信息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(FileAttachment attachment, RequestOptions? options = null);

    /// <summary>
    ///     发送文本消息到此私聊消息频道。
    /// </summary>
    /// <param name="text"> 要发送的文本。 </param>
    /// <param name="imageFileInfos"> 图片文件的信息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, RequestOptions? options = null);

    /// <summary>
    ///     发送卡片消息到此消息频道。
    /// </summary>
    /// <param name="card"> 要发送的卡片。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendCardAsync(ICard card, RequestOptions? options = null);

    /// <summary>
    ///     发送卡片消息到此消息频道。
    /// </summary>
    /// <param name="cards"> 要发送的卡片。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendCardsAsync(IEnumerable<ICard> cards, RequestOptions? options = null);
}
