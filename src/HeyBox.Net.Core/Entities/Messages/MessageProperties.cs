namespace HeyBox;

/// <summary>
///     提供用于修改 <see cref="HeyBox.IUserMessage"/> 的属性。
/// </summary>
/// <seealso cref="HeyBox.IUserMessage.ModifyAsync(System.Action{HeyBox.MessageProperties},HeyBox.RequestOptions)"/>
public class MessageProperties
{
    /// <summary>
    ///     获取或设置要设置到此消息的消息内容。
    /// </summary>
    /// <remarks>
    ///     修改此值为非空字符串可以修改消息的内容；不修改此值或将其设置为 <c>null</c> 可以保持消息的原内容。
    /// </remarks>
    public string? Content { get; set; }

    /// <summary>
    ///     获取或设置要设置到此消息的消息引用。
    /// </summary>
    public IMessageReference? Reference { get; set; }

    /// <summary>
    ///     获取或设置要设置到此消息的图像文件信息
    /// </summary>
    public List<FileAttachment>? ImageFileInfos { get; set; }
}
