namespace HeyBox.Interactions;

/// <summary>
///     为命令参数添加预设值。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
public class ChoiceAttribute : Attribute
{
    /// <summary>
    ///     获取选项名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取此选项的类型。
    /// </summary>
    public SlashCommandChoiceType Type { get; }

    /// <summary>
    ///     获取当选择此选项时将被使用的值。
    /// </summary>
    public object? Value { get; }

    private ChoiceAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     创建一个类型为 <see cref="SlashCommandChoiceType.String"/> 的参数选项。
    /// </summary>
    /// <param name="name"> 选项名称。 </param>
    /// <param name="value"> 选项的预设值。 </param>
    public ChoiceAttribute(string name, string value) : this(name)
    {
        Type = SlashCommandChoiceType.String;
        Value = value;
    }

    /// <summary>
    ///     创建一个类型为 <see cref="SlashCommandChoiceType.Integer"/> 的参数选项。
    /// </summary>
    /// <param name="name"> 选项名称。 </param>
    /// <param name="value"> 选项的预设值。 </param>
    public ChoiceAttribute(string name, int value) : this(name)
    {
        Type = SlashCommandChoiceType.Integer;
        Value = value;
    }

    /// <summary>
    ///     创建一个类型为 <see cref="SlashCommandChoiceType.Number"/> 的参数选项。
    /// </summary>
    /// <param name="name"> 选项名称。 </param>
    /// <param name="value"> 选项的预设值。 </param>
    public ChoiceAttribute(string name, double value) : this(name)
    {
        Type = SlashCommandChoiceType.Number;
        Value = value;
    }
}
