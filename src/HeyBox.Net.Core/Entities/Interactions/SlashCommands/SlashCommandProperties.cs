namespace HeyBox;

/// <summary>
///     Represents the base class to create/modify application commands.
/// </summary>
public class SlashCommandProperties
{
    /// <summary>
    ///     Gets or sets the name of this command.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets context types this command can be executed in.
    /// </summary>
    public HashSet<InteractionContextType>? ContextTypes { get; set; }

    /// <summary>
    ///    Gets or sets the description of this command.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the options for this command.
    /// </summary>
    public List<SlashCommandOptionProperties>? Options { get; set; }

    internal SlashCommandProperties() { }
}
