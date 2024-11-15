using System.Collections.Immutable;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using HeyBox.API;
using HeyBox.API.Rest;
using HeyBox.Net.Converters;

namespace HeyBox.Rest;

internal class MessageHelper
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    private static readonly Regex InlineCodeRegex = new(@"[^\\]?(`).+?[^\\](`)",
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

    private static readonly Regex BlockCodeRegex = new(@"[^\\]?(```).+?[^\\](```)",
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

    public static string SanitizeMessage(IMessage message)
    {
        string newContent = MentionUtils.Resolve(message, 0,
            TagHandling.FullName, TagHandling.FullName, TagHandling.FullName,
            TagHandling.FullName, TagHandling.FullName);
        return Format.StripMarkdown(newContent);
    }

    public static ImmutableArray<ITag> ParseTags(string text, IMessageChannel channel, IRoom? room,
        IReadOnlyCollection<IUser> userMentions)
    {
        ImmutableArray<ITag>.Builder tags = ImmutableArray.CreateBuilder<ITag>();
        int index = 0;
        int codeIndex = 0;

        while (true)
        {
            index = text.IndexOfAny(['@', '#'], index);
            if (index == -1)
                break;
            int endIndex = text.IndexOf('}', index + 1);
            if (endIndex == -1)
                break;
            if (CheckWrappedCode())
                break;
            string content = text.Substring(index, endIndex - index + 1);

            if (MentionUtils.TryParseUser(content, out uint userId))
            {
                IUser? mentionedUser = channel?.GetUserAsync(userId, CacheMode.CacheOnly).GetAwaiter().GetResult()
                    ?? userMentions.FirstOrDefault(x => x.Id == userId);
                tags.Add(new Tag<uint, IUser>(TagType.UserMention, index, content.Length, userId, mentionedUser));
            }
            else if (MentionUtils.TryParseRole(content, out ulong roleId))
            {
                IRole? role = room?.GetRole(roleId);
                tags.Add(new Tag<ulong, IRole>(TagType.RoleMention, index, content.Length, roleId, role));
            }
            else if (MentionUtils.TryParseChannel(content, out ulong channelId))
            {
                IRoomChannel? mentionedChannel = room?.GetChannelAsync(channelId).GetAwaiter().GetResult();
                tags.Add(new Tag<ulong, IChannel>(TagType.ChannelMention, index, content.Length, channelId, mentionedChannel));
            }
            else if (IEmote.TryParse(content, out IEmote? emoji))
                tags.Add(new Tag<string, IEmote>(TagType.Emoji, index, content.Length, emoji.Id, emoji));
            else //Bad Tag
            {
                index++;
                continue;
            }

            index = endIndex + 1;
        }

        index = 0;
        codeIndex = 0;
        while (true)
        {
            index = text.IndexOf(MentionUtils.MentionEveryone, index, StringComparison.Ordinal);
            if (index == -1) break;
            if (CheckWrappedCode()) break;
            int? tagIndex = FindIndex(tags, index);
            if (tagIndex.HasValue)
            {
                Tag<ulong,IRole> hereMention = new(TagType.EveryoneMention, index, MentionUtils.MentionEveryone.Length, 0, room?.EveryoneRole);
                tags.Insert(tagIndex.Value, hereMention);
            }
            index++;
        }

        index = 0;
        codeIndex = 0;
        while (true)
        {
            index = text.IndexOf(MentionUtils.MentionHere, index, StringComparison.Ordinal);
            if (index == -1) break;
            if (CheckWrappedCode()) break;
            int? tagIndex = FindIndex(tags, index);
            if (tagIndex.HasValue)
            {
                Tag<ulong,IRole> hereMention = new(TagType.HereMention, index, MentionUtils.MentionHere.Length, 0, room?.EveryoneRole);
                tags.Insert(tagIndex.Value, hereMention);
            }
            index++;
        }

        return tags.ToImmutable();


        // checks if the tag being parsed is wrapped in code blocks
        bool CheckWrappedCode()
        {
            // loop through all code blocks that are before the start of the tag
            while (codeIndex < index)
            {
                Match blockMatch = BlockCodeRegex.Match(text, codeIndex);
                if (blockMatch.Success)
                {
                    if (EnclosedInBlock(blockMatch))
                        return true;

                    // continue if the end of the current code was before the start of the tag
                    codeIndex += blockMatch.Groups[2].Index + blockMatch.Groups[2].Length;
                    if (codeIndex < index)
                        continue;

                    return false;
                }

                Match inlineMatch = InlineCodeRegex.Match(text, codeIndex);
                if (inlineMatch.Success)
                {
                    if (EnclosedInBlock(inlineMatch))
                        return true;

                    // continue if the end of the current code was before the start of the tag
                    codeIndex += inlineMatch.Groups[2].Index + inlineMatch.Groups[2].Length;
                    if (codeIndex < index)
                        continue;

                    return false;
                }

                return false;
            }

            return false;

            // util to check if the index of a tag is within the bounds of the codeblock
            bool EnclosedInBlock(Match m) => m.Groups[1].Index < index && index < m.Groups[2].Index;
        }
    }

    private static int? FindIndex(IReadOnlyList<ITag> tags, int index)
    {
        int i = 0;
        for (; i < tags.Count; i++)
        {
            ITag tag = tags[i];
            if (index < tag.Index)
                break; //Position before this tag
        }
        if (i > 0 && index < tags[i - 1].Index + tags[i - 1].Length)
            return null; //Overlaps tag before this
        return i;
    }

    public static async Task ModifyAsync(RestUserMessage message,
        Action<MessageProperties> func, BaseHeyBoxClient client, RequestOptions? options)
    {
        if (message.Channel is not IRoomChannel roomChannel)
            throw new NotSupportedException("Deleting a message from a non-room channel is not supported.");
        MessageProperties properties = new()
        {
            Content = message.Content,
            Reference = message.Reference,
            ImageFileInfos = message.ImageFileInfos is not null ? [..message.ImageFileInfos] : null
        };
        func(properties);
        ImmutableArray<ITag> tags = ParseTags(properties.Content, message.Channel, roomChannel.Room, []);
        bool hasMention = tags.Any(x => x.Type
            is TagType.UserMention or TagType.ChannelMention
            or TagType.RoleMention or TagType.EveryoneMention or TagType.HereMention);
        ImageFilesInfo imageFilesInfo = new()
        {
            FilesInfo = properties.ImageFileInfos?.Select(ChannelHelper.CreateImageFileInfo)?.ToArray() ?? []
        };
        ModifyChannelMessageParams args = new()
        {
            MessageId = message.Id,
            RoomId = roomChannel.RoomId,
            ChannelId = roomChannel.Id,
            MessageType = hasMention ? MessageType.MarkdownWithMention : MessageType.Markdown,
            Message = properties.Content,
            ReplyId = properties.Reference?.MessageId,
            Addition = JsonSerializer.Serialize(imageFilesInfo, _serializerOptions),
            AtUserId = [..tags.Where(tag => tag.Type == TagType.UserMention).OfType<Tag<uint, IUser>>().Select(x => x.Key)],
            AtRoleId = [..tags.Where(tag => tag.Type == TagType.RoleMention).OfType<Tag<ulong, IRole>>().Select(x => x.Key)],
            MentionChannelId = [..tags.Where(tag => tag.Type == TagType.ChannelMention).OfType<Tag<ulong, IChannel>>().Select(x => x.Key)],
        };
        await client.ApiClient.ModifyChannelMessageAsync(args, options);
    }

    public static async Task DeleteAsync(RestUserMessage message, BaseHeyBoxClient client, RequestOptions? options)
    {
        if (message.Channel is not IRoomChannel roomChannel)
            throw new NotSupportedException("Deleting a message from a non-room channel is not supported.");
        DeleteChannelMessageParams args = new()
        {
            RoomId = roomChannel.RoomId,
            ChannelId = roomChannel.Id,
            MessageId = message.Id
        };
        await client.ApiClient.DeleteChannelMessageAsync(args, options);
    }

    private static readonly JsonSerializerOptions CardJsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { CardConverterFactory.Instance },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static ImmutableArray<ICard> ParseCards(string json)
    {
        CardMessage? cardMessage = JsonSerializer.Deserialize<CardMessage>(json, CardJsonSerializerOptions);
        if (cardMessage is null)
            throw new InvalidOperationException("Failed to parse cards from the provided JSON.");
        return [..cardMessage.Data.Select(x => x.ToEntity())];
    }

    public static string SerializeCards(IEnumerable<ICard> cards)
    {
        const int maxCardCount = 3;
        IEnumerable<ICard> enumerable = cards as ICard[] ?? cards.ToArray();
        Preconditions.AtMost(enumerable.Count(), maxCardCount, nameof(cards),
            $"A max of {maxCardCount} cards can be included in a card message.");
        API.CardMessage message = new()
        {
            Data = [..enumerable.Select(c => c.ToModel())]
        };
        return JsonSerializer.Serialize(message, CardJsonSerializerOptions);
    }

    #region Reactions

    public static async Task AddReactionAsync(ulong messageId, ulong channelId, ulong roomId, IEmote emote,
        BaseHeyBoxClient client, RequestOptions? options)
    {
        ReplyReactionParams args = new()
        {
            MessageId = messageId,
            Emoji = emote.Id,
            IsAdd = true,
            ChannelId = channelId,
            RoomId = roomId,
        };
        await client.ApiClient.ReplyReactionAsync(args, options).ConfigureAwait(false);
    }

    public static Task AddReactionAsync(IMessage message, ITextChannel textChannel, IEmote emote,
        BaseHeyBoxClient client, RequestOptions? options) =>
        AddReactionAsync(message.Id, textChannel.Id, textChannel.RoomId, emote, client, options);

    public static async Task RemoveReactionAsync(ulong messageId, ulong channelId, ulong roomId, IEmote emote,
        BaseHeyBoxClient client, RequestOptions? options)
    {
        ReplyReactionParams args = new()
        {
            MessageId = messageId,
            Emoji = emote.Id,
            IsAdd = false,
            ChannelId = channelId,
            RoomId = roomId,
        };
        await client.ApiClient.ReplyReactionAsync(args, options).ConfigureAwait(false);
    }

    public static Task RemoveReactionAsync(IMessage message, ITextChannel textChannel, IEmote emote,
        BaseHeyBoxClient client, RequestOptions? options) =>
        RemoveReactionAsync(message.Id, textChannel.Id, textChannel.RoomId, emote, client, options);

    #endregion
}
