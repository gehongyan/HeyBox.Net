using HeyBox.API.Gateway;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

internal class SocketResolvableData
{
    private readonly Func<ulong, SocketRoomChannel> _channelResolver;
    private readonly Func<ulong, SocketRole> _roleResolver;
    private readonly Func<uint, SocketUser> _userResolver;
    private readonly Func<string, int, Attachment> _attachmentResolver;

    internal SocketResolvableData(SocketRoom room, CommandInfo commandInfo)
    {
        _channelResolver = x => room.AddOrUpdateChannel(x);
        _roleResolver = x => room.GetRole(x) ?? new SocketRole(room, x);
        _userResolver = room.AddOrUpdateUser;
        _attachmentResolver = (fileName, index) =>
        {
            Attachment attachment;
            if (commandInfo.Files?.FirstOrDefault(y => y.OptionIndex == index) is { } fileModel)
                attachment = new Attachment(AttachmentType.File, fileModel.Url, fileModel.Name, fileModel.Size);
            else if (commandInfo.Images?.FirstOrDefault(y => y.OptionIndex == index) is { } imageModel)
                attachment = new Attachment(AttachmentType.Image, imageModel.Url, imageModel.Name, imageModel.Size,
                    imageModel.Type, imageModel.Width, imageModel.Height);
            else
                throw new InvalidOperationException($"The attachment parameter information with index {index} cannot be found.");
            if (attachment.Filename != fileName)
                throw new InvalidOperationException($"The file name in the attachment parameter information with index {index} does not match the parameter value.");
            return attachment;
        };
    }

    public SocketRoomChannel ResolveChannel(ulong id) => _channelResolver(id);
    public SocketRole ResolveRole(ulong id) => _roleResolver(id);
    public SocketUser ResolveUser(uint id) => _userResolver(id);
    public Attachment ResolveAttachment(string fileName, int index) => _attachmentResolver(fileName, index);
}
