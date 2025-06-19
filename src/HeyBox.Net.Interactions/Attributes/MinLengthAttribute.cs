namespace HeyBox.Interactions;

/// <summary>
///     设置字符串类型参数允许的最小长度。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class MinLengthAttribute : Attribute
{
    /// <summary>
    ///     获取字符串类型参数允许的最小长度。
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     设置字符串类型参数允许的最小长度。
    /// </summary>
    /// <param name="length"> 允许的最小字符串长度。 </param>
    public MinLengthAttribute(int length)
    {
        Length = length;
    }
}
