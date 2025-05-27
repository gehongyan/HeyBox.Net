namespace HeyBox;

/// <summary>
///     表示一个 CSS 宽度的结构体。
/// </summary>
public readonly struct NodeWidth : IEquatable<NodeWidth>
{
    /// <summary>
    ///     获取宽度的字符串值。
    /// </summary>
    public string Value { get; } = string.Empty;

    /// <summary>
    ///     获取一个表示默认宽度的 <see cref="NodeWidth"/> 实例。
    /// </summary>
    public static NodeWidth Default { get; } = new(string.Empty);

    /// <summary>
    ///     使用指定的宽度字符串值初始化 <see cref="NodeWidth"/> 结构。
    /// </summary>
    /// <param name="value">宽度的字符串值。</param>
    private NodeWidth(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     通过宽度值创建 <see cref="NodeWidth"/> 实例。
    /// </summary>
    /// <param name="value"> 宽度值字符串。</param>
    /// <returns> 对应的 <see cref="NodeWidth"/> 实例。</returns>
    public static NodeWidth FromValue(string value) => new(value);

    /// <summary>
    ///     通过像素值创建 <see cref="NodeWidth"/> 实例。
    /// </summary>
    /// <param name="pixels"> 像素值，必须为非负整数。</param>
    /// <returns> 对应的 <see cref="NodeWidth"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException"> 当 <paramref name="pixels"/> 小于 0 时抛出。</exception>
    public static NodeWidth FromPixels(int pixels)
    {
        if (pixels < 0)
            throw new ArgumentOutOfRangeException(nameof(pixels), "Pixel value cannot be negative.");
        return new NodeWidth($"{pixels}px");
    }

    /// <summary>
    ///     通过百分比值创建 <see cref="NodeWidth"/> 实例。
    /// </summary>
    /// <param name="percentage"> 百分比值，必须为非负数。</param>
    /// <returns> 对应的 <see cref="NodeWidth"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException"> 当 <paramref name="percentage"/> 小于 0 时抛出。</exception>
    public static NodeWidth FromPercentage(double percentage)
    {
        if (percentage < 0)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage value must be between 0 and 100.");
        return new NodeWidth($"{percentage}%");
    }

    /// <inheritdoc cref="HeyBox.NodeWidth.Value" />
    public override string ToString() => Value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is NodeWidth other && Equals(other);

    /// <inheritdoc />
    public bool Equals(NodeWidth other) => Value == other.Value;

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// 确定两个 <see cref="NodeWidth"/> 实例是否相等。
    /// </summary>
    /// <param name="left">左侧的 <see cref="NodeWidth"/> 实例。</param>
    /// <param name="right">右侧的 <see cref="NodeWidth"/> 实例。</param>
    /// <returns>如果相等，则为 true；否则为 false。</returns>
    public static bool operator ==(NodeWidth left, NodeWidth right) => left.Equals(right);

    /// <summary>
    /// 确定两个 <see cref="NodeWidth"/> 实例是否不相等。
    /// </summary>
    /// <param name="left">左侧的 <see cref="NodeWidth"/> 实例。</param>
    /// <param name="right">右侧的 <see cref="NodeWidth"/> 实例。</param>
    /// <returns>如果不相等，则为 true；否则为 false。</returns>
    public static bool operator !=(NodeWidth left, NodeWidth right) => !(left == right);
}
