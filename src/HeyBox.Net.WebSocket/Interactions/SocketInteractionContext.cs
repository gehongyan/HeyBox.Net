using HeyBox.WebSocket;
using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     Represents a Web-Socket based context of an <see cref="IHeyBoxInteraction"/>.
/// </summary>
public class SocketInteractionContext<TInteraction> : IInteractionContext, IRouteMatchContainer
    where TInteraction : SocketInteraction
{
    /// <summary>
    ///     Gets the <see cref="HeyBoxSocketClient"/> that the command will be executed with.
    /// </summary>
    public HeyBoxSocketClient Client { get; }

    /// <inheritdoc />
    public ulong? RoomId { get; }

    /// <summary>
    ///     Gets the <see cref="SocketRoom"/> the command originated from.
    /// </summary>
    /// <remarks>
    ///     Will be null if the command is from a DM Channel.
    /// </remarks>
    public SocketRoom? Room { get; }

    /// <inheritdoc />
    public ulong? ChannelId { get; }

    /// <summary>
    ///     Gets the <see cref="ISocketMessageChannel"/> the command originated from.
    /// </summary>
    public ISocketMessageChannel? Channel { get; }

    /// <inheritdoc />
    public uint UserId { get; }

    /// <summary>
    ///     Gets the <see cref="SocketUser"/> who executed the command.
    /// </summary>
    public SocketUser? User { get; }

    /// <summary>
    ///     Gets the <see cref="SocketInteraction"/> the command was received with.
    /// </summary>
    public TInteraction Interaction { get; }

    /// <inheritdoc cref="IRouteMatchContainer.SegmentMatches"/>
    public IReadOnlyCollection<IRouteSegmentMatch> SegmentMatches { get; private set; } = [];

    /// <summary>
    ///     Initializes a new <see cref="SocketInteractionContext{TInteraction}"/>.
    /// </summary>
    /// <param name="client">The underlying client.</param>
    /// <param name="interaction">The underlying interaction.</param>
    public SocketInteractionContext(HeyBoxSocketClient client, TInteraction interaction)
    {
        Client = client;
        RoomId = (interaction.Channel as SocketRoomChannel)?.Room.Id;
        Room = (interaction.User as SocketRoomUser)?.Room;
        ChannelId = interaction.ChannelId;
        Channel = interaction.Channel;
        UserId = interaction.User.Id;
        User = interaction.User;
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
    IMessageChannel? IInteractionContext.Channel => Channel;

    /// <inheritdoc/>
    IUser? IInteractionContext.User => User;

    /// <inheritdoc/>
    IHeyBoxInteraction IInteractionContext.Interaction => Interaction;
}

/// <summary>
///     Represents a Web-Socket based context of an <see cref="IHeyBoxInteraction"/>
/// </summary>
public class SocketInteractionContext : SocketInteractionContext<SocketInteraction>
{
    /// <summary>
    ///     Initializes a new <see cref="SocketInteractionContext"/>
    /// </summary>
    /// <param name="client">The underlying client</param>
    /// <param name="interaction">The underlying interaction</param>
    public SocketInteractionContext(HeyBoxSocketClient client, SocketInteraction interaction)
        : base(client, interaction) { }
}
