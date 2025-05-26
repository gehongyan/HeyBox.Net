using System.Collections.Immutable;
using HeyBox.API.Rest;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的用户消息。
/// </summary>
public class RestUserMessage : RestMessage, IUserMessage
{
    private ImmutableArray<ICard> _cards = [];

    /// <inheritdoc />
    public override IReadOnlyCollection<ICard> Cards => _cards;

    internal RestUserMessage(BaseHeyBoxClient client, ulong id, MessageType messageType,
        IMessageChannel channel, IUser author, MessageSource source)
        : base(client, id, messageType, channel, author, source)
    {
    }

    internal static RestUserMessage Create(BaseHeyBoxClient client,
        IMessageChannel channel, IUser author,
        SendChannelMessageParams args, SendChannelMessageResponse model)
    {
        RestUserMessage entity = new(client, model.MessageId, args.MessageType, channel, author, MessageSource.User);
        entity.Update(args, model);
        return entity;
    }

    internal static RestUserMessage Create(BaseHeyBoxClient client,
        IMessageChannel channel, IUser author,
        SendUserMessageParams args, SendUserMessageResponse model)
    {
        RestUserMessage entity = new(client, model.MessageId, args.MessageType, channel, author, MessageSource.User);
        entity.Update(args, model);
        return entity;
    }

    /// <inheritdoc />
    internal override void Update(SendChannelMessageParams args, SendChannelMessageResponse model)
    {
        base.Update(args, model);

        if (Type == MessageType.Card)
            _cards = MessageHelper.ParseCards(args.Message);
    }

    /// <inheritdoc />
    internal override void Update(SendUserMessageParams args, SendUserMessageResponse model)
    {
        base.Update(args, model);

        if (Type == MessageType.Card)
            _cards = MessageHelper.ParseCards(args.Message);
    }

    /// <summary>
    ///     转换消息文本中的提及与表情符号为可读形式。
    /// </summary>
    /// <param name="startIndex"> 指定解析的起始位置。 </param>
    /// <param name="userHandling"> 指定用户提及标签的处理方式。 </param>
    /// <param name="channelHandling"> 指定频道提及标签的处理方式。 </param>
    /// <param name="roleHandling"> 指定角色提及标签的处理方式。 </param>
    /// <param name="everyoneHandling"> 指定全体成员与在线成员提及标签的处理方式。 </param>
    /// <param name="emojiHandling"> 指定表情符号标签的处理方式。 </param>
    /// <returns> 转换后的消息文本。 </returns>
    public string Resolve(int startIndex, TagHandling userHandling = TagHandling.Name,
        TagHandling channelHandling = TagHandling.Name, TagHandling roleHandling = TagHandling.Name,
        TagHandling everyoneHandling = TagHandling.Name, TagHandling emojiHandling = TagHandling.Name) =>
        MentionUtils.Resolve(this, startIndex,
            userHandling, channelHandling, roleHandling, everyoneHandling, emojiHandling);

    /// <inheritdoc />
    public string Resolve(TagHandling userHandling = TagHandling.Name,
        TagHandling channelHandling = TagHandling.Name, TagHandling roleHandling = TagHandling.Name,
        TagHandling everyoneHandling = TagHandling.Name, TagHandling emojiHandling = TagHandling.Name) =>
        MentionUtils.Resolve(this, 0,
            userHandling, channelHandling, roleHandling, everyoneHandling, emojiHandling);

    /// <inheritdoc />
    public Task ModifyAsync(Action<MessageProperties> func, RequestOptions? options = null) =>
        MessageHelper.ModifyAsync(this, func, Client, options);

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) => MessageHelper.DeleteAsync(this, Client, options);
}
