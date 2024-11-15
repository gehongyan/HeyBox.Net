namespace HeyBox;

/// <summary>
///     表示一个 <see cref="IModule"/> 的类型。
/// </summary>
public enum ModuleType
{
    /// <summary>
    ///     文本模块。
    /// </summary>
    Section,

    /// <summary>
    ///     标题模块。
    /// </summary>
    Header,

    /// <summary>
    ///     图片组模块。
    /// </summary>
    Images,

    /// <summary>
    ///     按钮组模块。
    /// </summary>
    ButtonGroup,

    /// <summary>
    ///     分割线模块。
    /// </summary>
    Divider,

    /// <summary>
    ///     倒计时模块。
    /// </summary>
    Countdown
}
