namespace HeyBox;

/// <summary>
///     表示频道的类型。
/// </summary>
public enum ChannelType
{
    /// <summary>
    ///     频道类型未指定。
    /// </summary>
    Unspecified = -1,

    /// <summary>
    ///     表示一个语音频道。
    /// </summary>
    Voice = 0,

    /// <summary>
    ///     表示一个文本频道。
    /// </summary>
    Text = 1,

    /// <summary>
    ///     表示一个公告频道。
    /// </summary>
    Announcement = 2,

    /// <summary>
    ///     表示一个分组频道。
    /// </summary>
    Category = 3,

    /// <summary>
    ///     表示一个临时频道。
    /// </summary>
    Temporary = 4,

    /// <summary>
    ///     表示一个临时频道管理器。
    /// </summary>
    TemporaryManager = 5,
}
