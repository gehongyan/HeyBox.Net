namespace HeyBox;

/// <summary>
///     表示一个斜线命令交互的选项。
/// </summary>
public interface ISlashCommandInteractionDataOption
{
    /// <summary>
    ///     获取此选项的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此选项的原始值。
    /// </summary>
    string RawValue { get; }

    /// <summary>
    ///     获取此选项的值。
    ///     <note>
    ///         此对象类型可能是 <see cref="SlashCommandOptionType"/> 中的任意一个选项类型。
    ///     </note>
    /// </summary>
    object? Value { get; }

    /// <summary>
    ///     获取此选项的类型。
    /// </summary>
    SlashCommandOptionType Type { get; }
}
