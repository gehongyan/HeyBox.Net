using HeyBox.API.Gateway;

namespace HeyBox.WebSocket;

/// <summary>
///     表示一个通过网关接收的斜线命令的数据。
/// </summary>
public class SocketSlashCommandData : SocketEntity<ulong>, ISlashCommandInteractionData
{
    /// <summary>
    ///     获取命令的名称。
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     获取用户提供的选项。
    /// </summary>
    public IReadOnlyCollection<SocketSlashCommandDataOption> Options { get; internal set; }

    internal SocketResolvableData ResolvableData { get; }

    internal SocketSlashCommandData(HeyBoxSocketClient client, CommandInfo model, SocketRoom room)
        : base(client, model.Id)
    {
        Name = string.Empty;
        Options = [];
        ResolvableData = new SocketResolvableData(room, model);
    }

    internal static SocketSlashCommandData Create(HeyBoxSocketClient client, CommandInfo model, SocketRoom room)
    {
        SocketSlashCommandData entity = new(client, model, room);
        entity.Update(model);
        return entity;
    }

    internal void Update(CommandInfo model)
    {
        Name = model.Name.TrimStart('/');
        if (model.Options is { } options)
            Options = [..options.Select((x, index) => new SocketSlashCommandDataOption(this, x, index))];

        IsPopulated = true;
    }

    IReadOnlyCollection<ISlashCommandInteractionDataOption> ISlashCommandInteractionData.Options => Options;
}
