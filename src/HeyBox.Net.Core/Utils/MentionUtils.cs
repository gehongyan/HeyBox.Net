using System.Globalization;
using System.Text;

namespace HeyBox;

/// <summary>
///     提供一组用于生成与解析提及标签的辅助方法。
/// </summary>
public static class MentionUtils
{
    private const int UserIdMaxLength = 10;
    private const int ChannelIdMaxLength = 20;
    private const int RoleIdMaxLength = 20;

    private const char SanitizeChar = '\u200b';

    internal static string MentionChannel(string id) => $"#{{id:{id}}}";

    /// <summary>
    ///     返回基于频道 ID 的格式化频道提及字符串。
    /// </summary>
    /// <returns> 频道提及字符串。 </returns>
    public static string MentionChannel(ulong id) => MentionChannel(id.ToString());

    internal static string MentionRole(string id) => $"@{{id:{id}}}";

    /// <summary>
    ///     返回基于角色 ID 的格式化频道提及字符串。
    /// </summary>
    /// <returns> 角色提及字符串。 </returns>
    public static string MentionRole(ulong id) => MentionRole(id.ToString());

    internal static string MentionUser(string id) => $"@{{id:{id}}}";

    /// <summary>
    ///     返回基于用户 ID 的格式化频道提及字符串。
    /// </summary>
    /// <returns> 用户提及字符串。 </returns>
    public static string MentionUser(uint id) => MentionUser(id.ToString());

    /// <summary>
    ///     返回全体成员提及字符串。
    /// </summary>
    public static string MentionEveryone => "@{all}";

    /// <summary>
    ///     返回在线成员提及字符串。
    /// </summary>
    public static string MentionHere => "@{here}";

    /// <summary>
    ///     将指定的用户提及字符串解析为用户 ID。
    /// </summary>
    /// <param name="text"> 要解析的用户提及字符串。 </param>
    /// <returns> 解析的用户 ID。 </returns>
    /// <exception cref="ArgumentException"> 无效的用户提及字符串格式。 </exception>
    public static uint ParseUser(string text)
    {
        if (TryParseUser(text, out uint id))
            return id;
        throw new ArgumentException(message: "Invalid mention format.", paramName: nameof(text));
    }

    /// <summary>
    ///     尝试解析指定的用户提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的用户提及字符串。 </param>
    /// <param name="userId"> 如果解析成功，则为用户 ID；否则为 <c>0</c>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool TryParseUser(string text, out uint userId)
    {
        // @{id:123}
        if (text is ['@', '{', 'i', 'd', ':', .. { Length: <= UserIdMaxLength } id, '}'])
        {
            if (uint.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out userId))
                return true;
        }
        userId = 0;
        return false;
    }

    /// <summary>
    ///     解析指定的频道提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的频道提及字符串。 </param>
    /// <returns> 解析的频道 ID。 </returns>
    /// <exception cref="ArgumentException"> 无效的频道提及字符串格式。 </exception>
    public static ulong ParseChannel(string text)
    {
        if (TryParseChannel(text, out ulong id))
            return id;
        throw new ArgumentException(message: "Invalid mention format.", paramName: nameof(text));
    }

    /// <summary>
    ///     尝试解析指定的频道提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的频道提及字符串。 </param>
    /// <param name="channelId"> 如果解析成功，则为频道 ID；否则为 <c>0</c>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool TryParseChannel(string text, out ulong channelId)
    {
        // #{id:123}
        if (text is ['#', '{', 'i', 'd', ':', .. { Length: <= ChannelIdMaxLength } id, '}'])
        {
            if (ulong.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out channelId))
                return true;
        }
        channelId = 0;
        return false;
    }

    /// <summary>
    ///     解析指定的角色提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的角色提及字符串。 </param>
    /// <returns> 解析的角色 ID。 </returns>
    /// <exception cref="ArgumentException"> 无效的角色提及字符串格式。 </exception>
    public static ulong ParseRole(string text)
    {
        if (TryParseRole(text, out ulong id))
            return id;
        throw new ArgumentException(message: "Invalid mention format.", paramName: nameof(text));
    }

    /// <summary>
    ///     尝试解析指定的角色提及字符串。
    /// </summary>
    /// <param name="text">T 要解析的角色提及字符串。 </param>
    /// <param name="roleId"> 如果解析成功，则为角色 ID；否则为 <c>0</c>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool TryParseRole(string text, out ulong roleId)
    {
        // @{id:123}
        if (text is ['@', '{', 'i', 'd', ':', .. { Length: <= RoleIdMaxLength } id, '}'])
        {
            if (ulong.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out roleId))
                return true;
        }
        roleId = 0;
        return false;
    }

    internal static string Resolve(IMessage msg, int startIndex, TagHandling userHandling, TagHandling channelHandling,
        TagHandling roleHandling, TagHandling everyoneHandling, TagHandling emojiHandling)
    {
        StringBuilder text = new(msg.Content[startIndex..]);
        IReadOnlyCollection<ITag> tags = msg.Tags;
        int indexOffset = -startIndex;

        foreach (ITag tag in tags)
        {
            if (tag.Index < startIndex) continue;

            string newText;
            switch (tag.Type)
            {
                case TagType.UserMention:
                    if (userHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveUserMention(tag, userHandling);
                    break;
                case TagType.ChannelMention:
                    if (channelHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveChannelMention(tag, channelHandling);
                    break;
                case TagType.RoleMention:
                    if (roleHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveRoleMention(tag, roleHandling);
                    break;
                case TagType.EveryoneMention:
                    if (everyoneHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveEveryoneMention(tag, everyoneHandling);
                    break;
                case TagType.HereMention:
                    if (everyoneHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveHereMention(tag, everyoneHandling);
                    break;
                case TagType.Emoji:
                    if (emojiHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveEmoji(tag, emojiHandling);
                    break;
                default:
                    newText = string.Empty;
                    break;
            }

            text.Remove(tag.Index + indexOffset, tag.Length);
            text.Insert(tag.Index + indexOffset, newText);
            indexOffset += newText.Length - tag.Length;
        }

        return text.ToString();
    }

    internal static string ResolveUserMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not IUser user)
            return string.Empty;

        IRoomUser? roomUser = user as IRoomUser;
        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => $"@{roomUser?.Nickname ?? user.Username}",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => $"{roomUser?.Nickname ?? user.Username}",
            TagHandling.Sanitize => MentionUser($"{SanitizeChar}{tag.Key}"),
            _ => string.Empty
        };
    }

    internal static string ResolveChannelMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not IChannel channel)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => $"#{channel.Name}",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => $"{channel.Name}",
            TagHandling.Sanitize => MentionChannel($"{SanitizeChar}{tag.Key}"),
            _ => string.Empty
        };
    }

    internal static string ResolveRoleMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not IRole role)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => $"@{role.Name}",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => $"{role.Name}",
            TagHandling.Sanitize => MentionRole($"{SanitizeChar}{tag.Key}"),
            _ => string.Empty
        };
    }

    internal static string ResolveEveryoneMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => "@全体成员",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => "全体成员",
            TagHandling.Sanitize => $"@{{{SanitizeChar}all}}",
            _ => string.Empty
        };
    }

    internal static string ResolveHereMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => "@在线成员",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => "在线成员",
            TagHandling.Sanitize => $"@{{{SanitizeChar}here}}",
            _ => ""
        };
    }

    internal static string ResolveEmoji(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not IEmote emoji)
            return string.Empty;

        return mode switch
        {
            TagHandling.Name or TagHandling.FullName => $":{emoji.Name}:",
            TagHandling.NameNoPrefix or TagHandling.FullNameNoPrefix => $"{emoji.Name}",
            TagHandling.Sanitize => emoji switch
            {
                RoomEmote emote => $"[custom{SanitizeChar}{emote.RoomId}_{SanitizeChar}{emote.Path}.{SanitizeChar}{emote.Extension}]",
                _ => $"[{SanitizeChar}{emoji.Group}_{SanitizeChar}{emoji.Id}]"
            },
            _ => ""
        };
    }
}
