namespace HeyBox;

/// <summary>
///     表示一个斜线命令选项的类型。
/// </summary>
public enum SlashCommandOptionType
{
    /// <summary>
    ///     字符串。
    /// </summary>
    String = 3,

    /// <summary>
    ///     整数。
    /// </summary>
    Integer = 4,

    /// <summary>
    ///     布尔值。
    /// </summary>
    Boolean = 5,

    /// <summary>
    ///     <see cref="HeyBox.IUser"/>。
    /// </summary>
    User = 6,

    /// <summary>
    ///     <see cref="HeyBox.IChannel"/>。
    /// </summary>
    Channel = 7,

    /// <summary>
    ///     <see cref="HeyBox.IRole"/>。
    /// </summary>
    Role = 8,

    /// <summary>
    ///     选择项。
    /// </summary>
    Selection = 9
}