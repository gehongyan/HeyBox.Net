namespace HeyBox;

/// <summary>
///     表示房间自定义问题的类型。
/// </summary>
[Flags]
public enum RoomCustomizationQuestionTypes
{
    /// <summary>
    ///     TODO
    /// </summary>
    None = 0,

    /// <summary>
    ///     TODO
    /// </summary>
    Multiple = 1 << 0,

    /// <summary>
    ///     TODO
    /// </summary>
    Required = 1 << 1
}
