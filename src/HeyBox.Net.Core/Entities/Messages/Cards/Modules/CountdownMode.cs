namespace HeyBox;

/// <summary>
///     表示一个 <see cref="HeyBox.CountdownModule"/> 的倒计时显示模式。
/// </summary>
public enum CountdownMode
{
    /// <summary>
    ///     倒计时器将以简单的文字显示倒计时时间，以日、时、分和秒的单位呈现。
    /// </summary>
    Default,

    /// <summary>
    ///     倒计时器将以类似翻页日历卡片的形式显示时间，以日、时、分和秒的单位呈现。
    /// </summary>
    Calendar,

    /// <summary>
    ///     倒计时器将以秒的形式显示时间。
    /// </summary>
    Second
}