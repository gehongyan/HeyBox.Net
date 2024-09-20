namespace HeyBox;

/// <summary>
///     表示一个斜线命令交互的数据。
/// </summary>
public interface ISlashCommandInteractionData : IHeyBoxInteractionData
{
    /// <summary>
    ///     获取用户提供的选项。
    /// </summary>
    IReadOnlyCollection<ISlashCommandInteractionDataOption> Options { get; }
}