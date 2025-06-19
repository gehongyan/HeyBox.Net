using HeyBox.WebSocket;
using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary> 表示基于 WebSocket 的 <see cref="IHeyBoxInteraction"/> 上下文。 </summary>
public class SocketInteractionContext<TInteraction> : IInteractionContext, IRouteMatchContainer
    where TInteraction : SocketInteraction
{
    /// <summary> 获取用于执行命令的 <see cref="HeyBoxSocketClient"/>。 </summary>
    public HeyBoxSocketClient Client { get; }

    /// <inheritdoc />
    public ulong? RoomId { get; }

    /// <summary> 获取命令来源的 <see cref="SocketRoom"/>。 </summary>
    /// <remarks> 如果命令来源为私聊频道，则为 <see langword="null"/>。 </remarks>
    public SocketRoom? Room { get; }

    /// <summary> 获取命令来源的 <see cref="ISocketMessageChannel"/>。 </summary>
    public ISocketMessageChannel Channel { get; }

    /// <inheritdoc />
    public uint UserId { get; }

    /// <summary> 获取执行命令的 <see cref="SocketUser"/>。 </summary>
    public SocketUser? User { get; }

    /// <inheritdoc />
    public ulong MessageId { get; }

    /// <summary> 获取接收到命令的 <see cref="SocketInteraction"/>。 </summary>
    public TInteraction Interaction { get; }

    /// <inheritdoc cref="IRouteMatchContainer.SegmentMatches"/>
    public IReadOnlyCollection<IRouteSegmentMatch> SegmentMatches { get; private set; } = [];

    /// <summary> 初始化 <see cref="SocketInteractionContext{TInteraction}"/>。 </summary>
    /// <param name="client"> 底层客户端。 </param>
    /// <param name="interaction"> 底层交互对象。 </param>
    public SocketInteractionContext(HeyBoxSocketClient client, TInteraction interaction)
    {
        Client = client;
        RoomId = (interaction.Channel as SocketRoomChannel)?.Room.Id;
        Room = (interaction.User as SocketRoomUser)?.Room;
        Channel = interaction.Channel;
        UserId = interaction.UserId;
        User = interaction.User;
        MessageId = interaction.MessageId;
        Interaction = interaction;
    }

    /// <inheritdoc/>
    public void SetSegmentMatches(IEnumerable<IRouteSegmentMatch> segmentMatches) => SegmentMatches = segmentMatches.ToImmutableArray();

    //IRouteMatchContainer
    /// <inheritdoc/>
    IEnumerable<IRouteSegmentMatch> IRouteMatchContainer.SegmentMatches => SegmentMatches;

    // IInteractionContext
    /// <inheritdoc/>
    IHeyBoxClient IInteractionContext.Client => Client;

    /// <inheritdoc/>
    IRoom? IInteractionContext.Room => Room;

    /// <inheritdoc/>
    IMessageChannel IInteractionContext.Channel => Channel;

    /// <inheritdoc/>
    IUser? IInteractionContext.User => User;

    /// <inheritdoc/>
    IHeyBoxInteraction IInteractionContext.Interaction => Interaction;
}

/// <summary> 表示基于 WebSocket 的 <see cref="IHeyBoxInteraction"/> 上下文。 </summary>
public class SocketInteractionContext : SocketInteractionContext<SocketInteraction>
{
    /// <summary> 初始化 <see cref="SocketInteractionContext"/>。 </summary>
    /// <param name="client"> 底层客户端。 </param>
    /// <param name="interaction"> 底层交互对象。 </param>
    public SocketInteractionContext(HeyBoxSocketClient client, SocketInteraction interaction)
        : base(client, interaction) { }
}
