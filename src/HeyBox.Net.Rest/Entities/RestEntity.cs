namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的具有唯一标识符的实体。
/// </summary>
/// <typeparam name="TId"> 唯一标识符的类型。 </typeparam>
public abstract class RestEntity<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
    internal BaseHeyBoxClient Client { get; }

    /// <inheritdoc />
    public TId Id { get; }

    internal RestEntity(BaseHeyBoxClient client, TId id)
    {
        Client = client;
        Id = id;
    }
}
