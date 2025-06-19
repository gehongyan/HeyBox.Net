namespace HeyBox.Interactions;

/// <summary>
///     表示斜线命令参数的选项。
/// </summary>
public class ParameterChoice
{
    /// <summary>
    ///     获取选项的名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取选项的值。
    /// </summary>
    public object? Value { get; }

    internal ParameterChoice(string name, object? value)
    {
        Name = name;
        Value = value;
    }
}
