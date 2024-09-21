namespace HeyBox.Interactions
{
    internal abstract class DefaultEntityTypeConverter<T> : TypeConverter<T> where T : class
    {
        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
            ISlashCommandInteractionDataOption option, IServiceProvider? services)
        {
            if (option.Value is T value)
                return Task.FromResult(TypeConverterResult.FromSuccess(value));
            else
                return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Provided input cannot be read as {nameof(IChannel)}"));
        }
    }

    internal class DefaultRoleConverter<T> : DefaultEntityTypeConverter<T> where T : class, IRole
    {
        public override SlashCommandOptionType GetHeyBoxType() => SlashCommandOptionType.Role;
    }

    internal class DefaultUserConverter<T> : DefaultEntityTypeConverter<T> where T : class, IUser
    {
        public override SlashCommandOptionType GetHeyBoxType() => SlashCommandOptionType.User;
    }

    internal class DefaultChannelConverter<T> : DefaultEntityTypeConverter<T> where T : class, IChannel
    {
        private readonly List<ChannelType>? _channelTypes;

        public DefaultChannelConverter()
        {
            var type = typeof(T);

            _channelTypes = true switch
            {
                _ when typeof(ITextChannel).IsAssignableFrom(type)
                    => [ChannelType.Text],

                _ => null
            };
        }

        public override SlashCommandOptionType GetHeyBoxType() => SlashCommandOptionType.Channel;

        public override void Write(SlashCommandOptionProperties properties, IParameterInfo parameter)
        {
            if (_channelTypes is not null)
                properties.ChannelTypes = _channelTypes;
        }
    }
}
