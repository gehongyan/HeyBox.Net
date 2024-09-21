namespace HeyBox;

/// <summary>
///     表示一个通用的 HeyBox 交互实体。
/// </summary>
public interface IHeyBoxInteraction : IEntity<ulong>
{
    /// <summary>
    ///     获取此交互的类型。
    /// </summary>
    InteractionType Type { get; }

    /// <summary>
    ///     获取此交互的数据。
    /// </summary>
    IHeyBoxInteractionData Data { get; }

    /// <summary>
    ///     获取执行此交互的用户的 ID。
    /// </summary>
    uint UserId { get; }

    /// <summary>
    ///     获取执行此交互的用户。
    /// </summary>
    IUser User { get; }

    /// <summary>
    ///     获取执行此交互所在的频道的 ID。
    /// </summary>
    ulong? ChannelId { get; }

    /// <summary>
    ///     获取执行此交互所在的房间的 ID。
    /// </summary>
    ulong? RoomId { get; }
}

/// <summary>
///     表示 HeyBox 交互的类型。
/// </summary>
public enum InteractionType
{
    /// <summary>
    ///     斜线命令。
    /// </summary>
    SlashCommand = 50
}
