namespace HeyBox.WebSocket;
using Model = API.Gateway.SenderInfo;

internal sealed class SocketGlobalUser : SocketUser
{
    private readonly object _lockObj = new();
    private ushort _references;

    /// <inheritdoc />
    public override string? Username { get; internal set; }

    /// <inheritdoc />
    public override bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public override string? Avatar { get; internal set; }

    /// <inheritdoc />
    public override string? AvatarDecorationType { get; internal set; }

    /// <inheritdoc />
    public override string? AvatarDecorationUrl { get; internal set; }

    /// <inheritdoc />
    public override int? Level { get; internal set; }

    /// <inheritdoc />
    internal override SocketGlobalUser GlobalUser => this;

    public SocketGlobalUser(HeyBoxSocketClient client, uint id)
        : base(client, id)
    {
    }

    internal static SocketGlobalUser Create(HeyBoxSocketClient client, ClientState state, Model model)
    {
        SocketGlobalUser entity = new(client, model.UserId);
        entity.Update(state, model);
        return entity;
    }

    internal void AddRef()
    {
        checked
        {
            lock (_lockObj)
            {
                _references++;
            }
        }
    }

    internal void RemoveRef(HeyBoxSocketClient client)
    {
        lock (_lockObj)
        {
            if (--_references <= 0)
                client.RemoveUser(Id);
        }
    }

}
