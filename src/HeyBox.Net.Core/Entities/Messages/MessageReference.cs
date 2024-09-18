using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个消息引用。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MessageReference : IQuote
{
    /// <summary>
    ///     使用指定的消息 ID 创建一个新的 <see cref="MessageReference"/> 实例。
    /// </summary>
    /// <param name="quotedMessageId"> 要引用的消息的 ID。 </param>
    public MessageReference(ulong quotedMessageId)
    {
        QuotedMessageId = quotedMessageId;
    }

    /// <inheritdoc />
    public ulong QuotedMessageId { get; }

    private string DebuggerDisplay => $"Quote: {QuotedMessageId}";
}
