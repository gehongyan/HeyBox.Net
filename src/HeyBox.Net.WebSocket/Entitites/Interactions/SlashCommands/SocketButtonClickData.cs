using HeyBox.API.Gateway;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的按钮点击的数据。
/// </summary>
public class SocketButtonClickData : SocketEntity<ulong>, IButtonClickInteractionData
{
    /// <inheritdoc />
    public string Text { get; private set; }

    /// <inheritdoc />
    public ButtonEvent Event { get; private set; }

    /// <inheritdoc />
    public string Value { get; private set; }

    internal SocketButtonClickData(HeyBoxSocketClient client, ulong id)
        : base(client, id)
    {
        Text = string.Empty;
        Value = string.Empty;
    }

    internal static SocketButtonClickData Create(HeyBoxSocketClient client, ulong sequence, CardMessageButtonClickEvent model)
    {
        SocketButtonClickData entity = new(client, sequence);
        entity.Update(model);
        return entity;
    }

    internal void Update(CardMessageButtonClickEvent model)
    {
        Text = model.Text;
        Event = model.Event;
        Value = model.Value;

        IsPopulated = true;
    }
}
