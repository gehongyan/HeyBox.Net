using HeyBox.API.Gateway;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的斜线命令的数据。
/// </summary>
public class SocketSlashCommandData : SocketEntity<ulong>, ISlashCommandInteractionData
{
    /// <summary>
    ///     获取用户提供的选项。
    /// </summary>
    public IReadOnlyCollection<SocketSlashCommandDataOption> Options { get; internal set; }

    internal SocketResolvableData ResolvableData { get; }

    internal SocketSlashCommandData(HeyBoxSocketClient client, ulong id, SocketRoom room)
        : base(client, id)
    {
        Options = [];
        ResolvableData = new SocketResolvableData(room);
    }

    internal static SocketSlashCommandData Create(HeyBoxSocketClient client, CommandInfo model, SocketRoom room)
    {
        SocketSlashCommandData entity = new(client, model.Id, room);
        entity.Update(model);
        return entity;
    }

    internal void Update(CommandInfo model)
    {
        Options = [..model.Options.Select(x => new SocketSlashCommandDataOption(this, x))];
    }

    IReadOnlyCollection<ISlashCommandInteractionDataOption> ISlashCommandInteractionData.Options => Options;
}
