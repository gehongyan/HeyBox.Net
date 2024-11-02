namespace HeyBox;

/// <summary>
///     表示一个通用的消息。
/// </summary>
public interface IMessage : IEntity<ulong>
{
    /// <summary>
    ///     获取此消息的类型。
    /// </summary>
    MessageType Type { get; }

    /// <summary>
    ///     获取此消息的来源。
    /// </summary>
    MessageSource Source { get; }

    /// <summary>
    ///     获取此消息的来源频道。
    /// </summary>
    IMessageChannel Channel { get; }

    /// <summary>
    ///     获取此消息的作者。
    /// </summary>
    IUser Author { get; }

    /// <summary>
    ///     获取此消息的内容。
    /// </summary>
    /// <remarks>
    ///     如果消息不是文本消息，则此属性可能为空或包含原始代码。
    /// </remarks>
    string Content { get; }

    /// <summary>
    ///     获取此消息的纯净内容。
    /// </summary>
    /// <returns>
    ///     此属性会对 <see cref="Content"/> 的内容进行两步操作： <br />
    ///     1. 使用 <see cref="HeyBox.IUserMessage.Resolve(HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling)"/>
    ///     方法解析所有标签的完整名称； <br />
    ///     2. 使用 <see cref="HeyBox.Format.StripMarkdown(System.String)"/> 清理所有 KMarkdown 格式字符。
    /// </returns>
    /// <seealso cref="HeyBox.IUserMessage.Resolve(HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling,HeyBox.TagHandling)"/>
    /// <seealso cref="HeyBox.Format.StripMarkdown(System.String)"/>
    string CleanContent { get; }

    /// <summary>
    ///     获取此消息的引用信息。
    /// </summary>
    IMessageReference? Reference { get; }

    /// <summary>
    ///     获取此消息中解析出的所有标签。
    /// </summary>
    IReadOnlyCollection<ITag> Tags { get; }

    #region Reactions

    /// <summary>
    ///     向此消息添加一个回应。
    /// </summary>
    /// <param name="emote"> 要用于向此消息添加回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示添加添加异步操作的任务。 </returns>
    Task AddReactionAsync(IEmote emote, RequestOptions? options = null);

    /// <summary>
    ///     从此消息中移除一个回应。
    /// </summary>
    /// <param name="emote"> 要从此消息移除的回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步移除操作的任务。 </returns>
    Task RemoveReactionAsync(IEmote emote, RequestOptions? options = null);

    #endregion
}
