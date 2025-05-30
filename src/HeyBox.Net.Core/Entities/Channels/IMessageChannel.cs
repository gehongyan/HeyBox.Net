using System.Drawing;

namespace HeyBox;

/// <summary>
///     表示一个通用的消息频道，可以用来发送和接收消息。
/// </summary>
public interface IMessageChannel : IChannel
{
    #region Send Messages

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="path"> 文件的路径。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="stream"> 文件的流。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IMessageReference? messageReference = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="attachment"> 文件的附件信息。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendFileAsync(FileAttachment attachment,
        IMessageReference? messageReference = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文本消息到此消息频道。
    /// </summary>
    /// <param name="text"> 要发送的文本。 </param>
    /// <param name="imageFileInfos"> 图片文件的信息。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, IMessageReference? messageReference = null, RequestOptions? options = null);

    /// <summary>
    ///     发送卡片消息到此消息频道。
    /// </summary>
    /// <param name="card"> 要发送的卡片。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendCardAsync(ICard card,
        IMessageReference? messageReference = null, RequestOptions? options = null);

    /// <summary>
    ///     发送卡片消息到此消息频道。
    /// </summary>
    /// <param name="cards"> 要发送的卡片。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    Task<IUserMessage> SendCardsAsync(IEnumerable<ICard> cards,
        IMessageReference? messageReference = null, RequestOptions? options = null);

    #endregion

    #region Delete Messages

    /// <summary>
    ///     删除一条消息。
    /// </summary>
    /// <param name="messageId"> 要删除的消息的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步删除操作的任务。 </returns>
    Task DeleteMessageAsync(ulong messageId, RequestOptions? options = null);

    /// <summary> 删除一条消息. </summary>
    /// <param name="message"> 要删除的消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步删除操作的任务。 </returns>
    Task DeleteMessageAsync(IMessage message, RequestOptions? options = null);

    #endregion
}
