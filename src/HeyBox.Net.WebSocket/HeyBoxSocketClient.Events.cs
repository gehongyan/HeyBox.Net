namespace HeyBox.WebSocket;

public partial class HeyBoxSocketClient
{
    #region General

    /// <summary>
    ///     当连接到黑盒语音网关时引发。
    /// </summary>
    public event Func<Task> Connected
    {
        add => _connectedEvent.Add(value);
        remove => _connectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _connectedEvent = new();

    /// <summary>
    ///     当与黑盒语音网关断开连接时引发。
    /// </summary>
    public event Func<Exception, Task> Disconnected
    {
        add => _disconnectedEvent.Add(value);
        remove => _disconnectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Exception, Task>> _disconnectedEvent = new();

    /// <summary>
    ///     当此 Bot 准备就绪以供用户代码访问时引发。
    /// </summary>
    public event Func<Task> Ready
    {
        add => _readyEvent.Add(value);
        remove => _readyEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<Task>> _readyEvent = new();

    /// <summary>
    ///     当网关延迟已更新时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="System.Int32"/> 参数是更新前的延迟（毫秒）。 </item>
    ///     <item> <see cref="System.Int32"/> 参数是更新后的延迟（毫秒）。 </item>
    ///     </list>
    /// </remarks>
    public event Func<int, int, Task> LatencyUpdated
    {
        add => _latencyUpdatedEvent.Add(value);
        remove => _latencyUpdatedEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<int, int, Task>> _latencyUpdatedEvent = new();

    #endregion


    #region Interactions

    /// <summary>
    ///     当交互被创建时引发，此事件涵盖了所有类型的交互。
    /// </summary>
    public event Func<SocketInteraction, Task> InteractionCreated
    {
        add => _interactionCreatedEvent.Add(value);
        remove => _interactionCreatedEvent.Remove(value);
    }
    internal readonly AsyncEvent<Func<SocketInteraction, Task>> _interactionCreatedEvent = new();

    /// <summary>
    ///     当用户使用斜线命令时引发。
    /// </summary>
    public event Func<SocketSlashCommand, Task> SlashCommandExecuted
    {
        add => _slashCommandExecuted.Add(value);
        remove => _slashCommandExecuted.Remove(value);
    }
    internal readonly AsyncEvent<Func<SocketSlashCommand, Task>> _slashCommandExecuted = new();

    #endregion
}
