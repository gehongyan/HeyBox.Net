namespace HeyBox;

/// <summary>
///     表示一个通用的消息引用。
/// </summary>
public interface IMessageReference
{
    /// <summary>
    ///     获取此引用所指向的消息的 ID。
    /// </summary>
    ulong MessageId { get; }
}
