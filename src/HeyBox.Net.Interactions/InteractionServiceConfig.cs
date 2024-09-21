namespace HeyBox.Interactions;

/// <summary>
///     表示一个 <see cref="InteractionService"/> 的配置类。
/// </summary>
public class InteractionServiceConfig
{
    /// <summary>
    ///     获取或设置将发送到 <see cref="InteractionService.Log"/> 事件的最低日志级别严重性。
    /// </summary>
    public LogSeverity LogLevel { get; set; } = LogSeverity.Info;

    /// <summary>
    ///     获取或设置命令应具有的默认 <see cref="RunMode"/>，如果在命令的特性或生成器上未指定时，将使用此默认值。
    /// </summary>
    public RunMode DefaultRunMode { get; set; } = RunMode.Async;

    /// <summary>
    ///     获取或设置是否 <see cref="RunMode.Sync"/> 命令应将异常传递给调用者。
    /// </summary>
    public bool ThrowOnError { get; set; } = true;

    /// <summary>
    ///     获取或设置将视为通配符的字符串表达式。
    /// </summary>
    public string WildCardExpression { get; set; } = "*";

    /// <summary>
    ///     获取或设置是否使用编译的 Lambda 表达式来创建模块实例和执行命令。此方法可以提高性能，但会增加内存消耗。
    /// </summary>
    public bool UseCompiledLambda { get; set; } = false;

    /// <summary>
    ///     获取或设置是否在每次命令执行时解析模块依赖项时应自动创建新的服务作用域。
    /// </summary>
    public bool AutoServiceScopes { get; set; } = true;
}
