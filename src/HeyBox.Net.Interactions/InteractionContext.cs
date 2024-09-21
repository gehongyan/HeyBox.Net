using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <inheritdoc cref="IInteractionContext"/>
public class InteractionContext : IInteractionContext, IRouteMatchContainer
{
    /// <inheritdoc/>
    public IHeyBoxClient Client { get; }

    /// <inheritdoc />
    public ulong? RoomId { get; }

    /// <inheritdoc/>
    public IRoom? Room { get; }

    /// <inheritdoc />
    public ulong? ChannelId { get; }

    /// <inheritdoc/>
    public IMessageChannel? Channel { get; }

    /// <inheritdoc />
    public uint UserId { get; }

    /// <inheritdoc/>
    public IUser? User { get; }
    /// <inheritdoc/>
    public IHeyBoxInteraction Interaction { get; }

    /// <inheritdoc cref="IRouteMatchContainer.SegmentMatches"/>
    public IReadOnlyCollection<IRouteSegmentMatch> SegmentMatches { get; private set; } = [];

    /// <summary>
    ///     初始化一个 <see cref="InteractionContext"/> 类的新实例。
    /// </summary>
    /// <param name="client"> HeyBox 客户端。 </param>
    /// <param name="interaction"> 交互事件。 </param>
    /// <param name="channel"> 交互事件发生的频道。 </param>
    public InteractionContext(IHeyBoxClient client, IHeyBoxInteraction interaction, IMessageChannel? channel = null)
    {
        Client = client;
        Interaction = interaction;
        RoomId = interaction.RoomId;
        Room = (interaction.User as IRoomUser)?.Room;
        ChannelId = interaction.ChannelId;
        Channel = channel;
        UserId = interaction.UserId;
        User = interaction.User;
        Interaction = interaction;
    }

    /// <inheritdoc/>
    public void SetSegmentMatches(IEnumerable<IRouteSegmentMatch> segmentMatches)
    {
        SegmentMatches = segmentMatches.ToImmutableArray();
    }

    //IRouteMatchContainer
    /// <inheritdoc/>
    IEnumerable<IRouteSegmentMatch> IRouteMatchContainer.SegmentMatches => SegmentMatches;
}
