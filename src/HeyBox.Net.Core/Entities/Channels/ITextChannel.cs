namespace HeyBox;

/// <summary>
///     表示房间中一个通用的具有文字聊天能力的频道，可以发送和接收消息。
/// </summary>
public interface ITextChannel : INestedChannel, IMentionable, IMessageChannel;
