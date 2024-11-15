namespace HeyBox;

/// <summary>
///     表示一个通用的 HeyBox 斜线命令交互实体。
/// </summary>
public interface ISlashCommandInteraction : IHeyBoxInteraction
{
    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.Data" />
    new ISlashCommandInteractionData Data { get; }
}

/// <summary>
///     表示一个通用的 HeyBox 按钮点击交互实体。
/// </summary>
public interface IButtonClickInteraction : IHeyBoxInteraction
{
    /// <inheritdoc cref="HeyBox.IHeyBoxInteraction.Data" />
    new IButtonClickInteractionData Data { get; }
}
