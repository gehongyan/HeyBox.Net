namespace HeyBox.Interactions;

/// <summary>
///     Create an Slash Application Command.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SlashCommandAttribute : Attribute
{
    /// <summary>
    ///     Gets the name of the Slash Command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the description of the Slash Command.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Gets the run mode this command gets executed with.
    /// </summary>
    public RunMode RunMode { get; }

    /// <summary>
    ///     Register a method as a Slash Command.
    /// </summary>
    /// <param name="name">Name of the command.</param>
    /// <param name="description">Description of the command.</param>
    /// <param name="runMode">Set the run mode of the command.</param>
    public SlashCommandAttribute(string name, string description, RunMode runMode = RunMode.Default)
    {
        Name = name;
        Description = description;
        RunMode = runMode;
    }
}
