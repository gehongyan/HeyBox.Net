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

    public static async Task<IUserMessage> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, string path, string filename, AttachmentType type, Size? imageSize, IMessageReference? messageReference, RequestOptions? options)
    {
        using FileAttachment file = new(path, filename, type, imageSize);
        return await SendFileAsync(channel, client, file, messageReference, options);
    }

    public static async Task<IUserMessage> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, Stream stream, string filename, AttachmentType type, Size? imageSize, IMessageReference? messageReference, RequestOptions? options)
    {
        using FileAttachment file = new(stream, filename, type, imageSize);
        return await SendFileAsync(channel, client, file, messageReference, options);
    }

    public static async Task<IUserMessage> SendFileAsync(ITextChannel channel,
        BaseHeyBoxClient client, FileAttachment attachment, IMessageReference? messageReference, RequestOptions? options)
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
                    FileName = attachment.Filename
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

        async Task<IUserMessage> SendAttachmentAsync(MessageType messageType)
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
                MessageType = messageType,
                Message = attachment.Uri.OriginalString,
                ReplyId = messageReference?.MessageId,
                Addition = JsonSerializer.Serialize(imageFilesInfo, _serializerOptions),
                AtUserId = [],
                AtRoleId = [],
                MentionChannelId = []
            };
            SendChannelMessageResponse model = await client.ApiClient.SendChannelMessageAsync(args, options);
            return CreateMessageEntity(client, channel, args, model, [attachment]);
        }
    }

    public static async Task<IUserMessage> SendTextAsync(ITextChannel channel, BaseHeyBoxClient client,
        string text, IEnumerable<FileAttachment>? imageFileInfos, IMessageReference? messageReference, RequestOptions? options)
    {
        ImmutableArray<ITag> tags = MessageHelper.ParseTags(text, channel, channel.Room, []);
        bool hasMention = tags.Any(x => x.Type
            is TagType.UserMention or TagType.ChannelMention
            or TagType.RoleMention or TagType.EveryoneMention or TagType.HereMention);
        IReadOnlyCollection<FileAttachment>? images = imageFileInfos is not null ? [..imageFileInfos] : null;
        ImageFilesInfo imageFilesInfo = new()
        {
            FilesInfo = images?.Select(CreateImageFileInfo)?.ToArray() ?? []
        };
        SendChannelMessageParams args = new()
        {
            RoomId = channel.RoomId,
            ChannelType = channel.Type,
            ChannelId = channel.Id,
            MessageType = hasMention ? MessageType.MarkdownWithMention : MessageType.Markdown,
            Message = text,
            ReplyId = messageReference?.MessageId,
            Addition = JsonSerializer.Serialize(imageFilesInfo, _serializerOptions),
            AtUserId = [..tags.Where(tag => tag.Type == TagType.UserMention).OfType<Tag<uint, IUser>>().Select(x => x.Key)],
            AtRoleId = [..tags.Where(tag => tag.Type == TagType.RoleMention).OfType<Tag<ulong, IRole>>().Select(x => x.Key)],
            MentionChannelId = [..tags.Where(tag => tag.Type == TagType.ChannelMention).OfType<Tag<ulong, IChannel>>().Select(x => x.Key)],
        };
        SendChannelMessageResponse model = await client.ApiClient.SendChannelMessageAsync(args, options);
        return CreateMessageEntity(client, channel, args, model, images);
    }

    public static Task<IUserMessage> SendCardAsync(ITextChannel channel, BaseHeyBoxClient client,
        ICard card, IMessageReference? messageReference, RequestOptions? options) =>
        SendCardsAsync(channel, client, [card], messageReference, options);

    public static async Task<IUserMessage> SendCardsAsync(ITextChannel channel, BaseHeyBoxClient client,
        IEnumerable<ICard> cards, IMessageReference? messageReference, RequestOptions? options)
    {
        SendChannelMessageParams args = new()
        {
            RoomId = channel.RoomId,
            ChannelType = channel.Type,
            ChannelId = channel.Id,
            MessageType = MessageType.Card,
            Message = MessageHelper.SerializeCards(cards),
            ReplyId = messageReference?.MessageId,
            Addition = "{}",
            AtUserId = [],
            AtRoleId = [],
            MentionChannelId = []
        };
        SendChannelMessageResponse model = await client.ApiClient.SendChannelMessageAsync(args, options);
        return CreateMessageEntity(client, channel, args, model, null);
    }

    internal static ImageFileInfo CreateImageFileInfo(FileAttachment attachment) =>
        new()
        {
            Url = attachment.Uri?.OriginalString,
            Width = attachment.ImageSize?.Width,
            Height = attachment.ImageSize?.Height
        };

    private static RestUserMessage CreateMessageEntity(BaseHeyBoxClient client, IMessageChannel channel,
        SendChannelMessageParams args, SendChannelMessageResponse model, IReadOnlyCollection<FileAttachment>? imageFileInfos)
    {
        if (client.CurrentUser is null)
            throw new InvalidOperationException("The client must have a current user.");
        RestUserMessage message = RestUserMessage.Create(client, channel, client.CurrentUser, args, model);
        message.Update(imageFileInfos);
        return message;
    }

}
