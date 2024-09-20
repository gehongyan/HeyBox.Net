using System.Text.Json;

namespace HeyBox.Net.Queue;

/// <summary>
///     表示一个提供新的 <see cref="IMessageQueue"/> 实例的委托。
/// </summary>
public delegate BaseMessageQueue MessageQueueProvider(Func<string, JsonElement, Task> eventHandler);
