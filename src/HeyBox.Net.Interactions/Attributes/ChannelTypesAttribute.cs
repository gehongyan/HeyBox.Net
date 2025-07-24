using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     指定 <see cref="SlashCommandOptionType.Channel"/> 选项允许的目标频道类型。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class ChannelTypesAttribute : Attribute
{
    /// <summary>
    ///     获取此选项允许的频道类型。
    /// </summary>
    public IReadOnlyCollection<ChannelType> ChannelTypes { get; }

    /// <summary>
    ///     指定 <see cref="SlashCommandOptionType.Channel"/> 选项允许的目标频道类型。
    /// </summary>
    /// <param name="channelTypes"> 此选项允许的频道类型。 </param>
    public ChannelTypesAttribute(params ChannelType[] channelTypes)
    {
        if (channelTypes is null)
            throw new ArgumentNullException(nameof(channelTypes));

        ChannelTypes = channelTypes.ToImmutableArray();
    }
}
