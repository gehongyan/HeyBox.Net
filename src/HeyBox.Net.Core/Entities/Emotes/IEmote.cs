namespace HeyBox;

/// <summary>
///     表示一个通用的表情符号。
/// </summary>
public interface IEmote : IEntity<ulong>
{
    /// <summary>
    ///     获取此表情符号的显示名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此表情符号的路径。
    /// </summary>
    ulong Path { get; }

    /// <summary>
    ///     获取此表情符号的扩展名。
    /// </summary>
    string Extension { get; }

    /// <summary>
    ///     获取此表情符号的创建时间。
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    ///     获取此表情符号的创建者。
    /// </summary>
    public IRoomUser Creator { get; }
}
