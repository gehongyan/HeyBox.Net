using Model = HeyBox.API.Gateway.CommandInfoOption;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的斜线命令的数据选项。
/// </summary>
public class SocketSlashCommandDataOption : ISlashCommandInteractionDataOption
{
    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public string RawValue { get; private set; }

    /// <inheritdoc />
    public object? Value { get; private set; }

    /// <inheritdoc />
    public SlashCommandOptionType Type { get; private set; }

    internal SocketSlashCommandDataOption(SocketSlashCommandData data, Model model, int index)
    {
        Name = model.Name;
        RawValue = model.Value;
        Type = model.Type;
        switch (Type)
        {
            case SlashCommandOptionType.String or SlashCommandOptionType.Selection:
                Value = model.Value;
                break;
            case SlashCommandOptionType.Integer when long.TryParse(model.Value, out long integer):
                Value = integer;
                break;
            case SlashCommandOptionType.Boolean when bool.TryParse(model.Value, out bool boolean):
                Value = boolean;
                break;
            case SlashCommandOptionType.User when uint.TryParse(model.Value, out uint userId):
                Value = data.ResolvableData.ResolveUser(userId);
                break;
            case SlashCommandOptionType.Channel when ulong.TryParse(model.Value, out ulong channelId):
                Value = data.ResolvableData.ResolveChannel(channelId);
                break;
            case SlashCommandOptionType.Role when ulong.TryParse(model.Value, out ulong roleId):
                Value = data.ResolvableData.ResolveRole(roleId);
                break;
            case SlashCommandOptionType.Image:
            case SlashCommandOptionType.File:
                Value = data.ResolvableData.ResolveAttachment(model.Value, index);
                break;
            default:
                throw new NotSupportedException($"Unsupported option type: {Type}");
        }
    }
}
