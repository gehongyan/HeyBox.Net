namespace HeyBox.Interactions;

internal abstract class DefaultUInt32EntityReader<T> : TypeReader<T>
    where T : class, IEntity<uint>
{
    protected abstract Task<T?> GetEntity(uint id, IInteractionContext ctx);

    public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option,
        IServiceProvider? services)
    {
        if (!uint.TryParse(option, out uint id))
            return TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{option} isn't a valid ID thus cannot be converted into {typeof(T).Name}");

        var result = await GetEntity(id, context).ConfigureAwait(false);

        return result is not null ?
            TypeConverterResult.FromSuccess(result) : TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{option} must be a valid {typeof(T).Name} ID to be parsed.");
    }

    public override Task<string?> SerializeAsync(object obj, IServiceProvider services) => Task.FromResult((obj as IEntity<uint>)?.Id.ToString());
}

internal abstract class DefaultUInt64EntityReader<T> : TypeReader<T>
    where T : class, IEntity<ulong>
{
    protected abstract Task<T?> GetEntity(ulong id, IInteractionContext ctx);

    public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option,
        IServiceProvider? services)
    {
        if (!ulong.TryParse(option, out ulong id))
            return TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{option} isn't a valid ID thus cannot be converted into {typeof(T).Name}");

        var result = await GetEntity(id, context).ConfigureAwait(false);

        return result is not null ?
            TypeConverterResult.FromSuccess(result) : TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{option} must be a valid {typeof(T).Name} ID to be parsed.");
    }

    public override Task<string?> SerializeAsync(object obj, IServiceProvider services) => Task.FromResult((obj as IEntity<ulong>)?.Id.ToString());
}

internal sealed class DefaultUserReader<T> : DefaultUInt32EntityReader<T>
    where T : class, IUser
{
    protected override async Task<T?> GetEntity(uint id, IInteractionContext ctx) => await ctx.Client.GetUserAsync(id, CacheMode.CacheOnly).ConfigureAwait(false) as T;
}

internal sealed class DefaultChannelReader<T> : DefaultUInt64EntityReader<T>
    where T : class, IChannel
{
    protected override async Task<T?> GetEntity(ulong id, IInteractionContext ctx) => await ctx.Client.GetChannelAsync(id, CacheMode.CacheOnly).ConfigureAwait(false) as T;
}

internal sealed class DefaultRoleReader<T> : DefaultUInt64EntityReader<T>
    where T : class, IRole
{
    protected override Task<T?> GetEntity(ulong id, IInteractionContext ctx) => Task.FromResult(ctx.Room?.GetRole(id) as T);
}
