using System.Text.Json;

namespace HeyBox.Net.Queue.SynchronousImmediate;

/// <summary>
///     表示一个同步处理消息队列。
/// </summary>
/// <remarks>
///     此消息队列会在接收到网关事件后调用
///     <see cref="HeyBox.Net.Queue.SynchronousImmediate.SynchronousImmediateMessageQueue.EnqueueAsync(System.UInt64,System.String,System.Text.Json.JsonElement,System.DateTimeOffset,System.Threading.CancellationToken)"/>
///     时立即使用构造函数中传入的 <c>eventHandler</c> 委托同步进行处理，处理完成后，该方法才会返回。
/// </remarks>
public class SynchronousImmediateMessageQueue : BaseMessageQueue
{
    /// <inheritdoc />
    public SynchronousImmediateMessageQueue(Func<ulong, string, JsonElement, Task> eventHandler)
        : base(eventHandler)
    {
    }

    /// <inheritdoc />
    public override async Task EnqueueAsync(ulong sequence, string type, JsonElement payload,
        DateTimeOffset timestamp, CancellationToken cancellationToken = default)
    {
        await EventHandler(sequence, type, payload).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    /// <inheritdoc />
    public override Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
