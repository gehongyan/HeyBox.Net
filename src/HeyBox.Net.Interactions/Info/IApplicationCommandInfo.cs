namespace HeyBox.Interactions;

/// <summary>
///     表示可注册到黑盒语音的 <see cref="InteractionService"/> 命令。
/// </summary>
public interface IApplicationCommandInfo
{
    /// <summary>
    ///     获取此命令的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此命令的类型。
    /// </summary>
    ApplicationCommandType CommandType { get; }

    /// <summary>
    ///     获取此命令可执行的上下文类型。
    /// </summary>
    public IReadOnlyCollection<InteractionContextType> ContextTypes { get; }
}
