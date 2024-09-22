namespace HeyBox.Interactions;

/// <summary>
///     Customize the displayed value of a slash command choice enum. Only works with the default enum type converter.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ChoiceValueAttribute : Attribute
{
    /// <summary>
    ///     Gets the name of the parameter.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Modify the default value of a Slash Command parameter.
    /// </summary>
    /// <param name="value">Value of the parameter.</param>
    public ChoiceValueAttribute(string value)
    {
        Value = value;
    }
}