using Model = HeyBox.API.Gateway.CardMessageButtonClickEvent;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的按钮点击命令。
/// </summary>
public class SocketButtonClick : SocketInteraction, IButtonClickInteraction
{
    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.Data" />
    public new SocketButtonClickData Data { get; }

    internal SocketButtonClick(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user, ulong sequence)
        : base(client, sequence, channel, user, model.MessageId)
    {
        Type = InteractionType.ButtonClick;
        Data = SocketButtonClickData.Create(client, sequence, model);
    }

    internal static new SocketInteraction Create(HeyBoxSocketClient client, Model model, SocketTextChannel channel, SocketRoomUser user, ulong sequence)
    {
        SocketButtonClick entity = new(client, model, channel, user, sequence);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        Data.Update(model);

        IsPopulated = true;
    }

    IButtonClickInteractionData IButtonClickInteraction.Data => Data;

    IHeyBoxInteractionData IHeyBoxInteraction.Data => Data;
}
