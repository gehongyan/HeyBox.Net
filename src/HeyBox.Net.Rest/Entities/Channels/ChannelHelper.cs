using System.Collections.Immutable;
using System.Drawing;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.API.Rest;

namespace HeyBox.Rest;

internal static class ChannelHelper
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public static async Task<Cacheable<IUserMessage, ulong>> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, string path, string filename, AttachmentType type, Size? imageSize, IQuote? quote, RequestOptions? options)
    {
        using FileAttachment file = new(path, filename, type, imageSize);
        return await SendFileAsync(channel, client, file, quote, options);
    }

    public static async Task<Cacheable<IUserMessage, ulong>> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, Stream stream, string filename, AttachmentType type, Size? imageSize, IQuote? quote, RequestOptions? options)
    {
        using FileAttachment file = new(stream, filename, type, imageSize);
        return await SendFileAsync(channel, client, file, quote, options);
    }

    public static async Task<Cacheable<IUserMessage, ulong>> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, FileAttachment attachment, IQuote? quote, RequestOptions? options)
    {
        switch (attachment.Mode)
        {
            case CreateAttachmentMode.FilePath:
            case CreateAttachmentMode.Stream:
            {
                if (attachment.Uri is not null) break;
                if (attachment.Stream is null)
                    throw new ArgumentNullException(nameof(attachment.Stream), "The stream cannot be null.");
                CreateAssetParams createAssetParams = new()
                {
                    File = attachment.Stream,
                    FileName = attachment.FileName
                };
                CreateAssetResponse assetResponse = await client.ApiClient
                    .CreateAssetAsync(createAssetParams, options).ConfigureAwait(false);
                attachment.Uri = assetResponse.Url;
                attachment.Dispose();
            }
                break;
            case CreateAttachmentMode.AssetUri:
                if (attachment.Uri is null || !UrlValidation.ValidateHeyBoxAssetUrl(attachment.Uri.OriginalString))
                    throw new ArgumentException("The uri cannot be blank.", nameof(attachment.Uri));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attachment.Mode), attachment.Mode, "Unknown attachment mode");
        }

        return attachment.Type switch
        {
            AttachmentType.Image => await SendAttachmentAsync(MessageType.Image).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(attachment.Type), attachment.Type, "Unknown attachment type")
        };

        async Task<Cacheable<IUserMessage, ulong>> SendAttachmentAsync(MessageType messageType)
        {
            ImageFilesInfo imageFilesInfo = new()
            {
                FilesInfo = [CreateImageFileInfo(attachment)]
            };
            SendChannelMessageParams args = new()
            {
                RoomId = channel.RoomId,
                ChannelType = channel.Type,
                ChannelId = channel.Id,
                MsgType = messageType,
                Message = attachment.Uri.OriginalString,
                HeyChatAckId = string.Empty,
                ReplyId = quote?.QuotedMessageId,
                Addition = JsonSerializer.Serialize(imageFilesInfo, _serializerOptions),
                AtUserId = [],
                AtRoleId = [],
                MentionChannelId = []
            };
            SendChannelMessageResponse model = await client.ApiClient.SendChannelMessageAsync(args, options);
            return new Cacheable<IUserMessage, ulong>(null, model.MessageId, false, () => Task.FromResult<IUserMessage?>(null));
        }
    }

    public static async Task<Cacheable<IUserMessage, ulong>> SendTextAsync(ITextChannel channel, BaseHeyBoxClient client,
        string text, IEnumerable<FileAttachment>? imageFileInfos, IQuote? quote, RequestOptions? options)
    {
        ImmutableArray<ITag> tags = MessageHelper.ParseTags(text, channel.Room);
        bool hasMention = tags.Any(x => x.Type
            is TagType.UserMention or TagType.ChannelMention
            or TagType.RoleMention or TagType.EveryoneMention or TagType.HereMention);
        ImageFilesInfo imageFilesInfo = new()
        {
            FilesInfo = imageFileInfos?.Select(CreateImageFileInfo)?.ToArray() ?? []
        };
        SendChannelMessageParams args = new()
        {
            RoomId = channel.RoomId,
            ChannelType = channel.Type,
            ChannelId = channel.Id,
            MsgType = hasMention ? MessageType.MarkdownWithMention : MessageType.Markdown,
            Message = text,
            HeyChatAckId = string.Empty,
            ReplyId = quote?.QuotedMessageId,
            Addition = JsonSerializer.Serialize(imageFilesInfo, _serializerOptions),
            AtUserId = [..tags.Where(tag => tag.Type == TagType.UserMention).OfType<Tag<uint, IUser>>().Select(x => x.Key)],
            AtRoleId = [..tags.Where(tag => tag.Type == TagType.RoleMention).OfType<Tag<ulong, IRole>>().Select(x => x.Key)],
            MentionChannelId = [..tags.Where(tag => tag.Type == TagType.ChannelMention).OfType<Tag<ulong, IChannel>>().Select(x => x.Key)],
        };
        SendChannelMessageResponse model = await client.ApiClient.SendChannelMessageAsync(args, options);
        return new Cacheable<IUserMessage, ulong>(null, model.MessageId, false, () => Task.FromResult<IUserMessage?>(null));
    }

    private static ImageFileInfo CreateImageFileInfo(FileAttachment attachment) =>
        new()
        {
            Url = attachment.Uri?.OriginalString,
            Width = attachment.ImageSize?.Width,
            Height = attachment.ImageSize?.Height
        };
}
