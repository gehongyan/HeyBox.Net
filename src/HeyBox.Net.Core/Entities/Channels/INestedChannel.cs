namespace HeyBox;

/// <summary>
///     表示一个通用的嵌套频道，即可以嵌套在分组频道中的房间频道。
/// </summary>
public interface INestedChannel : IRoomChannel
{
    /// <summary>
    ///     获取此频道是否为私密频道。
    /// </summary>
    bool IsPrivate { get; }

    /// <summary>
    ///     获取此嵌套频道在房间频道列表中所属的分组频道的 ID。
    /// </summary>
    /// <remarks> 如果当前频道不属于任何分组频道，则会返回 <c>null</c>。 </remarks>
    ulong? CategoryId { get; }

    /// <summary>
    ///     指示此嵌套频道的权限是否与其所属分组频道同步。
    /// </summary>
    /// <remarks>
    ///     如果权限同步，则此属性返回 <c>true</c>；如果权限不同步，则返回 <c>false</c>。
    /// </remarks>
    bool IsPermissionSynced { get; }
}
