namespace HeyBox;

/// <summary> 表示用于存储 CustomId 通配符匹配的对象。 </summary>
internal record RouteSegmentMatch : IRouteSegmentMatch
{
    /// <inheritdoc/>
    public string Value { get; }

    public RouteSegmentMatch(string value)
    {
        Value = value;
    }
}
