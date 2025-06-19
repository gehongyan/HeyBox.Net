namespace HeyBox.Interactions;

/// <summary>
///     设置数字类型参数允许的最小值。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class MinValueAttribute : Attribute
{
    /// <summary>
    ///     获取允许的最小值。
    /// </summary>
    public double Value { get; }

    /// <summary>
    ///     设置数字类型参数允许的最小值。
    /// </summary>
    /// <param name="value"> 允许的最小值。 </param>
    public MinValueAttribute(double value)
    {
        Value = value;
    }
}
