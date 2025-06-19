namespace HeyBox.Interactions;

/// <summary>
///     自定义斜线命令选项枚举的显示值。仅适用于默认枚举类型转换器。
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ChoiceValueAttribute : Attribute
{
    /// <summary>
    ///     获取参数值。
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     修改斜线命令参数的默认值。
    /// </summary>
    /// <param name="value"> 参数值。 </param>
    public ChoiceValueAttribute(string value)
    {
        Value = value;
    }
}
