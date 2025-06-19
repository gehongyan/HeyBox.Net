namespace HeyBox.Interactions;

/// <summary>
///     创建一个斜线应用命令。
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SlashCommandAttribute : Attribute
{
    /// <summary>
    ///     获取斜线命令的名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取斜线命令的描述。
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     获取此命令执行时的运行模式。
    /// </summary>
    public RunMode RunMode { get; }

    /// <summary>
    ///     注册一个方法为斜线命令。
    /// </summary>
    /// <param name="name"> 命令名称。 </param>
    /// <param name="description"> 命令描述。 </param>
    /// <param name="runMode"> 设置命令的运行模式。 </param>
    public SlashCommandAttribute(string name, string? description = null, RunMode runMode = RunMode.Default)
    {
        Name = name;
        Description = description;
        RunMode = runMode;
    }
}
