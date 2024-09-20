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

    internal SocketSlashCommandDataOption(SocketSlashCommandData data, Model model)
    {
        Name = model.Name;
        RawValue = model.Value;
        Type = model.Type;
        Value = Type switch
        {
            SlashCommandOptionType.String or SlashCommandOptionType.Selection => model.Value,
            SlashCommandOptionType.Integer when long.TryParse(model.Value, out long integer) => integer,
            SlashCommandOptionType.Boolean when bool.TryParse(model.Value, out bool boolean) => boolean,
            SlashCommandOptionType.User when uint.TryParse(model.Value, out uint userId) => data.ResolvableData.ResolveUser(userId),
            SlashCommandOptionType.Channel when ulong.TryParse(model.Value, out ulong channelId) => data.ResolvableData.ResolveChannel(channelId),
            SlashCommandOptionType.Role when ulong.TryParse(model.Value, out ulong roleId) => data.ResolvableData.ResolveRole(roleId),
            _ => throw new NotSupportedException($"Unsupported option type: {Type}")
        };
    }
}
