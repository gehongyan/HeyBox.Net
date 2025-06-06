using System.Diagnostics.CodeAnalysis;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个基于网关的回应。
/// </summary>
public class SocketReaction : IReaction
{
    /// <summary>
    ///     获取添加此回应的用户的 ID。
    /// </summary>
    public uint UserId { get; }

    /// <summary>
    ///     获取添加此回应的用户。
    /// </summary>
    /// <remarks>
    ///     如果要获取的用户实体不存在于缓存中，则此属性将返回 <see langword="null"/>。
    /// </remarks>
    public IUser? User { get; internal set; }

    /// <summary>
    ///     获取此回应所对应的消息的 ID。
    /// </summary>
    public ulong MessageId { get; }

    /// <summary>
    ///     获取此回应所对应的消息。
    /// </summary>
    /// <remarks>
    ///     如果要获取的消息实体不存在于缓存中，则此属性将返回 <see langword="null"/>。
    /// </remarks>
    public IMessage? Message { get; internal set; }

    /// <summary>
    ///     获取此回应所在的消息频道的 ID。
    /// </summary>
    public ulong ChannelId { get; }

    /// <summary>
    ///     获取此回应所在的消息频道。
    /// </summary>
    /// <remarks>
    ///     如果要获取的频道实体不存在于缓存中，则此属性将返回 <see langword="null"/>。
    /// </remarks>
    public ISocketMessageChannel? Channel { get; }

    /// <summary>
    ///     获取此回应所在的消息频道的 ID。
    /// </summary>
    public ulong? RoomId { get; }

    /// <summary>
    ///     获取此回应所在的消息频道。
    /// </summary>
    public SocketRoom? Room { get; internal set; }

    /// <inheritdoc />
    public IEmote Emote { get; }

    internal SocketReaction(ulong? roomId, SocketRoom? room, ulong channelId, ISocketMessageChannel? channel,
        ulong messageId, IMessage? message, uint userId, IUser? user, IEmote emoji)
    {
        RoomId = roomId;
        Room = room;
        ChannelId = channelId;
        Channel = channel;
        MessageId = messageId;
        Message = message;
        UserId = userId;
        User = user;
        Emote = emoji;
    }

    internal static SocketReaction Create(ClientState state, API.Gateway.ReactionEvent model)
    {
        IEmote emote = IEmote.Parse(model.Emoji);
        if (emote is RoomEmote roomEmote)
        {
            if (state.GetRoom(model.RoomId)?.Emotes.SingleOrDefault(x => x.Path == roomEmote.Path) is { } entity)
                emote = entity;
            else
            {
                SocketRoom? room = state.GetRoom(model.ChannelId);
                roomEmote.Room = room;
                roomEmote.Creator = room?.GetUser(model.UserId);
            }
        }
        return new SocketReaction(
            model.RoomId, state.GetRoom(model.RoomId),
            model.ChannelId, state.GetChannel(model.ChannelId) as ISocketMessageChannel,
            model.MessageId, null,
            model.UserId, state.GetUser(model.UserId),
            emote);
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (obj is not SocketReaction otherReaction) return false;
        return UserId == otherReaction.UserId
            && MessageId == otherReaction.MessageId
            && Emote.Equals(otherReaction.Emote);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = UserId.GetHashCode();
            hashCode = (hashCode * 397) ^ MessageId.GetHashCode();
            hashCode = (hashCode * 397) ^ Emote.GetHashCode();
            return hashCode;
        }
    }
}
