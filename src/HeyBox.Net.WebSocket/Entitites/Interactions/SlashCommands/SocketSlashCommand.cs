namespace HeyBox.WebSocket;
using Model = API.Gateway.CommandInfo;

/// <summary>
///     表示一个通过网关接收的斜线命令。
/// </summary>
public class SocketSlashCommand : SocketInteraction, ISlashCommandInteraction, IHeyBoxInteraction
{
    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.Data" />
    public new SocketSlashCommandData Data { get; }

    internal SocketSlashCommand(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user)
        : base(client, model.Id, channel, user)
    {
        Data = SocketSlashCommandData.Create(client, model, channel.Room);
    }

    internal static new SocketInteraction Create(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user)
    {
        SocketSlashCommand entity = new(client, model, channel, user);
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
