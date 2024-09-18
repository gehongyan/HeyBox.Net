using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace HeyBox.Rest;

internal class MessageHelper
{
    private static readonly Regex InlineCodeRegex = new(@"[^\\]?(`).+?[^\\](`)",
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

    private static readonly Regex BlockCodeRegex = new(@"[^\\]?(```).+?[^\\](```)",
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

    public static ImmutableArray<ITag> ParseTags(string text, IRoom? room)
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

            if (MentionUtils.TryParseRole(content, out ulong roleId))
            {
                tags.Add(new Tag<ulong, IRole>(TagType.RoleMention, index, content.Length, roleId, null));
            }
            else if (MentionUtils.TryParseUser(content, out uint userId))
            {
                tags.Add(new Tag<uint, IUser>(TagType.UserMention, index, content.Length, userId, null));
            }
            else if (MentionUtils.TryParseChannel(content, out ulong channelId))
            {
                tags.Add(new Tag<ulong, IChannel>(TagType.ChannelMention, index, content.Length, channelId, null));
            }
            // else if (Emote.TryParse(content, out var emoji))
            //     tags.Add(new Tag<Emote>(TagType.Emoji, index, content.Length, emoji.Id, emoji));
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
}
