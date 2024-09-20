namespace HeyBox;

/// <summary>
///     表示一个通用的频道。
/// </summary>
public interface IChannel : IEntity<ulong>
{
    /// <summary>
    ///     获取此频道的名称。
    /// </summary>
    string? Name { get; }
}
