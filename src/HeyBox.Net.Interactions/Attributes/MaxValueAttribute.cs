namespace HeyBox.Interactions;

/// <summary>
///     设置数字类型参数允许的最大值。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class MaxValueAttribute : Attribute
{
    /// <summary>
    ///     获取允许的最大值。
    /// </summary>
    public double Value { get; }

    /// <summary>
    ///     设置数字类型参数允许的最大值。
    /// </summary>
    /// <param name="value"> 允许的最大值。 </param>
    public MaxValueAttribute(double value)
    {
        Value = value;
    }
}
