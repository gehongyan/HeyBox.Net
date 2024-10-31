namespace HeyBox.WebSocket;

public abstract partial class BaseSocketClient
{
    #region Reactions

    /// <summary>
    ///     当服务器内的消息上被添加了新的回应时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是被添加了回应的可缓存消息。如果缓存中存在此消息实体，那么该结构内包含该
    ///         <see cref="HeyBox.IUserMessage"/> 消息；否则，包含 <see cref="System.UInt64"/> 消息 ID，以供按需下载实体。
    ///     </item>
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是消息所在的频道。如果缓存中存在此频道实体，那么该结构内包含该
    ///         <see cref="HeyBox.WebSocket.SocketTextChannel"/> 频道；否则，包含 <see cref="System.UInt64"/> 频道 ID，以供按需下载实体。
    ///     </item>
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是添加了此回应的可缓存服务器用户。如果缓存中存在此服务器用户实体，那么该结构内包含该
    ///         <see cref="HeyBox.WebSocket.SocketRoomUser"/> 服务器用户；否则，包含 <see cref="System.UInt32"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     <item> <see cref="HeyBox.WebSocket.SocketReaction"/> 参数是被添加的回应。 </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<IUserMessage, ulong>, Cacheable<SocketTextChannel, ulong>, Cacheable<SocketRoomUser, uint>, SocketReaction, Task> ReactionAdded
    {
        add => _reactionAddedEvent.Add(value);
        remove => _reactionAddedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<IUserMessage, ulong>, Cacheable<SocketTextChannel, ulong>, Cacheable<SocketRoomUser, uint>, SocketReaction, Task>> _reactionAddedEvent = new();

    /// <summary>
    ///     当服务器内的消息上存在的回应被用户移除时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是被移除了回应的可缓存消息。如果缓存中存在此消息实体，那么该结构内包含该
    ///         <see cref="HeyBox.IUserMessage"/> 消息；否则，包含 <see cref="System.UInt64"/> 消息 ID，以供按需下载实体。
    ///     </item>
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是消息所在的频道。如果缓存中存在此频道实体，那么该结构内包含该
    ///         <see cref="HeyBox.WebSocket.SocketTextChannel"/> 频道；否则，包含 <see cref="System.UInt64"/> 频道 ID，以供按需下载实体。
    ///     </item>
    ///     <item>
    ///         <see cref="HeyBox.Cacheable{TEntity,TId}"/> 参数是移除了此回应的可缓存服务器用户。如果缓存中存在此服务器用户实体，那么该结构内包含该
    ///         <see cref="HeyBox.WebSocket.SocketRoomUser"/> 服务器用户；否则，包含 <see cref="System.UInt32"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     <item> <see cref="HeyBox.WebSocket.SocketReaction"/> 参数是被移除的回应。 </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<IUserMessage, ulong>, Cacheable<SocketTextChannel, ulong>, Cacheable<SocketRoomUser, uint>, SocketReaction, Task> ReactionRemoved
    {
        add => _reactionRemovedEvent.Add(value);
        remove => _reactionRemovedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<IUserMessage, ulong>, Cacheable<SocketTextChannel, ulong>, Cacheable<SocketRoomUser, uint>, SocketReaction, Task>> _reactionRemovedEvent = new();

    #endregion

    #region Users

    /// <summary>
    ///     当用户加入房间时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.WebSocket.SocketRoomUser"/> 参数是加入房间的房间用户。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketRoomUser, Task> UserJoined
    {
        add => _userJoinedEvent.Add(value);
        remove => _userJoinedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketRoomUser, Task>> _userJoinedEvent = new();

    /// <summary>
    ///     当用户离开房间时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.WebSocket.SocketRoomUser"/> 参数是离开房间的房间用户。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketRoomUser, Task> UserLeft
    {
        add => _userLeftEvent.Add(value);
        remove => _userLeftEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketRoomUser, Task>> _userLeftEvent = new();

    #endregion

    #region Rooms

    /// <summary>
    ///     当当前用户新加入服务器时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.WebSocket.SocketRoom"/> 参数是当前用户新加入的服务器。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketRoom, Task> JoinedRoom
    {
        add => _joinedRoomEvent.Add(value);
        remove => _joinedRoomEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketRoom, Task>> _joinedRoomEvent = new();

    /// <summary>
    ///     当当前用户离开服务器时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="HeyBox.WebSocket.SocketRoom"/> 参数是当前用户离开的服务器。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketRoom, Task> LeftRoom
    {
        add => _leftRoomEvent.Add(value);
        remove => _leftRoomEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketRoom, Task>> _leftRoomEvent = new();

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
