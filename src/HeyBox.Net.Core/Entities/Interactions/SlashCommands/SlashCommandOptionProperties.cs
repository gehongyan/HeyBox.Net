using System.Text.RegularExpressions;

namespace HeyBox;

/// <summary>
///     Represents a slash commands option for making slash commands.
/// </summary>
public class SlashCommandOptionProperties
{
    private string _name = string.Empty;
    private string _description = string.Empty;

    /// <summary>
    ///     Gets or sets the name of this option.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            EnsureValidOptionName(value);
            _name = value;
        }
    }

    /// <summary>
    ///     Gets or sets the description of this option.
    /// </summary>
    public string Description
    {
        get => _description;
        set
        {
            EnsureValidOptionDescription(value);
            _description = value;
        }
    }

    /// <summary>
    ///     Gets or sets the type of this option.
    /// </summary>
    public SlashCommandOptionType Type { get; set; }

    /// <summary>
    ///     Gets or sets whether or not this options is the first required option for the user to complete. only one option can be default.
    /// </summary>
    public bool? IsDefault { get; set; }

    /// <summary>
    ///     Gets or sets if the option is required.
    /// </summary>
    public bool? IsRequired { get; set; }

    /// <summary>
    ///     Gets or sets the smallest number value the user can input.
    /// </summary>
    public double? MinValue { get; set; }

    /// <summary>
    ///     Gets or sets the largest number value the user can input.
    /// </summary>
    public double? MaxValue { get; set; }

    /// <summary>
    ///     Gets or sets the minimum allowed length for a string input.
    /// </summary>
    public int? MinLength { get; set; }

    /// <summary>
    ///     Gets or sets the maximum allowed length for a string input.
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    ///     Gets or sets the choices for string and int types for the user to pick from.
    /// </summary>
    public List<SlashCommandOptionChoiceProperties> Choices { get; set; } = [];

    /// <summary>
    ///     Gets or sets if this option is a subcommand or subcommand group type, these nested options will be the parameters.
    /// </summary>
    public List<SlashCommandOptionProperties> Options { get; set; } = [];

    /// <summary>
    ///     Gets or sets the allowed channel types for this option.
    /// </summary>
    public List<ChannelType> ChannelTypes { get; set; } = [];

    private static void EnsureValidOptionName(string? name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name), $"{nameof(Name)} cannot be null.");

        if (name.Length > 32)
            throw new ArgumentOutOfRangeException(nameof(name), "Name length must be less than or equal to 32.");

        if (!Regex.IsMatch(name, @"^[-_\p{L}\p{N}\p{IsDevanagari}\p{IsThai}]{1,32}$"))
            throw new ArgumentException(@"Name must match the regex ^[-_\p{L}\p{N}\p{IsDevanagari}\p{IsThai}]{1,32}$", nameof(name));

        if (name.Any(char.IsUpper))
            throw new FormatException("Name cannot contain any uppercase characters.");
    }

    private static void EnsureValidOptionDescription(string description)
    {
        switch (description.Length)
        {
            case > 100:
                throw new ArgumentOutOfRangeException(nameof(description),
                    "Description length must be less than or equal to 100.");
            case 0:
                throw new ArgumentOutOfRangeException(nameof(description), "Description length must at least 1.");
        }
    }
}
