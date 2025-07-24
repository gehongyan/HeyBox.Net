namespace HeyBox;

/// <summary> 表示用于存储 CustomId 通配符匹配的对象。 </summary>
public interface IRouteSegmentMatch
{
    /// <summary> 获取此通配符匹配捕获的值。 </summary>
    /// <returns> 此通配符的值。 </returns>
    string Value { get; }
}
