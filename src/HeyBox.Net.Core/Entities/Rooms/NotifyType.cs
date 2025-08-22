namespace HeyBox;

/// <summary>
///     表示房间应如何通知用户。
/// </summary>
public enum NotifyType
{
    /// <summary>
    ///     通知用户所有消息。
    /// </summary>
    AcceptAll = 1,

    /// <summary>
    ///     通知提及用户的消息。
    /// </summary>
    OnlyMentioned = 2,
}
