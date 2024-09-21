namespace HeyBox.Interactions;

/// <summary>
///     Supported types of pre-defined parameter choices.
/// </summary>
public enum SlashCommandChoiceType
{
    /// <summary>
    ///     HeyBox type for <see cref="string"/>.
    /// </summary>
    String,

    /// <summary>
    ///     HeyBox type for <see cref="int"/>.
    /// </summary>
    Integer,

    /// <summary>
    ///     HeyBox type for <see cref="double"/>.
    /// </summary>
    Number
}
