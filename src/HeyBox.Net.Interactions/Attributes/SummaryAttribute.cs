namespace HeyBox.Interactions;

/// <summary>
///     自定义斜线命令参数的名称和描述。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class SummaryAttribute : Attribute
{
    /// <summary>
    ///     获取参数名称。
    /// </summary>
    public string? Name { get; } = null;

    /// <summary>
    ///     获取参数描述。
    /// </summary>
    public string? Description { get; } = null;

    /// <summary>
    ///     修改斜线命令参数的默认名称和描述。
    /// </summary>
    /// <param name="name"> 参数名称。 </param>
    /// <param name="description"> 参数描述。 </param>
    public SummaryAttribute(string? name = null, string? description = null)
    {
        Name = name;
        Description = description;
    }
}
