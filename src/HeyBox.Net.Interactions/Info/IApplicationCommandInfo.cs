namespace HeyBox.Interactions;

/// <summary>
///     Represents a <see cref="InteractionService"/> command that can be registered to HeyBox.
/// </summary>
public interface IApplicationCommandInfo
{
    /// <summary>
    ///     Gets the name of this command.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Gets the type of this command.
    /// </summary>
    ApplicationCommandType CommandType { get; }

    /// <summary>
    ///     Gets the context types this command can be executed in.
    /// </summary>
    public IReadOnlyCollection<InteractionContextType> ContextTypes { get; }
}
