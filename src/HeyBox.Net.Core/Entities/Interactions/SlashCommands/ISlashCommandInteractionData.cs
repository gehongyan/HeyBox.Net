namespace HeyBox;

/// <summary>
///     表示一个斜线命令交互的数据。
/// </summary>
public interface ISlashCommandInteractionData : IHeyBoxInteractionData
{
    /// <summary>
    ///     获取命令的唯一标识符。
    /// </summary>
    ulong Id { get; }

    /// <summary>
    ///     获取命令的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取用户提供的选项。
    /// </summary>
    IReadOnlyCollection<ISlashCommandInteractionDataOption> Options { get; }
}
