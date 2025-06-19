namespace HeyBox;

/// <summary> 表示一次交互的上下文。 </summary>
public interface IInteractionContext
{
    /// <summary> 获取用于处理本次交互的客户端。 </summary>
    IHeyBoxClient Client { get; }

    /// <summary> 获取用于处理本次交互的应用命令信息。 </summary>
    ulong? RoomId { get; }

    /// <summary> 获取本次交互来源的群组。 </summary>
    /// <remarks> 如果交互来源为私聊频道或为上下文命令交互，则为 <see langword="null"/>。 </remarks>
    IRoom? Room { get; }

    /// <summary> 获取本次交互来源的频道。 </summary>
    IMessageChannel Channel { get; }

    /// <summary> 获取触发本次交互的用户 ID。 </summary>
    uint UserId { get; }

    /// <summary> 获取触发本次交互事件的用户。 </summary>
    IUser? User { get; }

    /// <summary> 获取本次交互来源的消息 ID。 </summary>
    ulong MessageId { get; }

    /// <summary> 获取底层交互对象。 </summary>
    IHeyBoxInteraction Interaction { get; }
}
