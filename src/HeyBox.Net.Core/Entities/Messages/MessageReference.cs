using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个消息引用。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MessageReference : IMessageReference
{
    /// <summary>
    ///     使用指定的消息 ID 创建一个新的 <see cref="MessageReference"/> 实例。
    /// </summary>
    /// <param name="messageId"> 要引用的消息的 ID。 </param>
    public MessageReference(ulong messageId)
    {
        MessageId = messageId;
    }

    /// <inheritdoc />
    public ulong MessageId { get; }

    private string DebuggerDisplay => $"messageReference: {MessageId}";
}
