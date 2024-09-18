using System.Drawing;

namespace HeyBox;

/// <summary>
///     表示一个通用的消息频道，可以用来发送和接收消息。
/// </summary>
public interface IMessageChannel : IChannel
{
    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="path"> 文件的路径。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="quote"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, ulong>> SendFileAsync(string path, string? filename = null,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IQuote? quote = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="stream"> 文件的流。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 文件的媒体类型。 </param>
    /// <param name="imageSize"> 图片文件的图像尺寸。 </param>
    /// <param name="quote"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, ulong>> SendFileAsync(Stream stream, string filename,
        AttachmentType type = AttachmentType.Image, Size? imageSize = null, IQuote? quote = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文件到此消息频道。
    /// </summary>
    /// <param name="attachment"> 文件的附件信息。 </param>
    /// <param name="quote"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, ulong>> SendFileAsync(FileAttachment attachment,
        IQuote? quote = null, RequestOptions? options = null);

    /// <summary>
    ///     发送文本消息到此消息频道。
    /// </summary>
    /// <param name="text"> 要发送的文本。 </param>
    /// <param name="imageFileInfos"> 图片文件的信息。 </param>
    /// <param name="quote"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, ulong>> SendTextAsync(string text,
        IEnumerable<FileAttachment>? imageFileInfos = null, IQuote? quote = null, RequestOptions? options = null);
}
