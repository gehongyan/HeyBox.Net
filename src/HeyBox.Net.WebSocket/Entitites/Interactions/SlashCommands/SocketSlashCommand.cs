using Model = HeyBox.API.Gateway.CommandInfo;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的斜线命令。
/// </summary>
public class SocketSlashCommand : SocketInteraction, ISlashCommandInteraction
{
    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.Data" />
    public new SocketSlashCommandData Data { get; }

    internal SocketSlashCommand(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user, ulong messageId)
        : base(client, model.Id, channel, user, messageId)
    {
        Type = InteractionType.SlashCommand;
        Data = SocketSlashCommandData.Create(client, model, channel.Room);
    }

    internal static new SocketInteraction Create(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user, ulong messageId)
    {
        SocketSlashCommand entity = new(client, model, channel, user, messageId);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        Data.Update(model);

        IsPopulated = true;
    }

    /// <inheritdoc/>
    ISlashCommandInteractionData ISlashCommandInteraction.Data => Data;

    /// <inheritdoc/>
    IHeyBoxInteractionData IHeyBoxInteraction.Data => Data;
}
