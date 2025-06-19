namespace HeyBox;

/// <summary> 表示用于临时存储组件 CustomId 通配符匹配的容器。 </summary>
public interface IRouteMatchContainer
{
    /// <summary> 获取此容器中捕获的路由片段集合。 </summary>
    /// <returns> 捕获的路由片段集合。 </returns>
    IEnumerable<IRouteSegmentMatch> SegmentMatches { get; }

    /// <summary> 设置此容器的 <see cref="SegmentMatches"/> 属性。 </summary>
    /// <param name="segmentMatches"> 捕获的路由片段集合。 </param>
    void SetSegmentMatches(IEnumerable<IRouteSegmentMatch> segmentMatches);
}
