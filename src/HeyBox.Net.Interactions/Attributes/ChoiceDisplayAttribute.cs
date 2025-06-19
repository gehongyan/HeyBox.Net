namespace HeyBox.Interactions;

/// <summary>
///     自定义斜线命令选项枚举的显示值。仅适用于默认枚举类型转换器。
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ChoiceDisplayAttribute : Attribute
{
    /// <summary>
    ///     获取参数名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     修改斜线命令参数的默认名称和描述。
    /// </summary>
    /// <param name="name"> 参数名称。 </param>
    public ChoiceDisplayAttribute(string name)
    {
        Name = name;
    }
}
