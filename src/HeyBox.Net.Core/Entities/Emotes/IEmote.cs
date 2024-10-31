namespace HeyBox;

/// <summary>
///     表示一个通用的表情符号。
/// </summary>
public interface IEmote
{
    /// <summary>
    ///     获取此表情符号的分组。
    /// </summary>
    string Group { get; }

    /// <summary>
    ///     获取此表情符号的显示名称。
    /// </summary>
    string? Name { get; }
}
