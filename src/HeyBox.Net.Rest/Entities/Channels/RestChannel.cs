namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的频道。
/// </summary>
public class RestChannel : RestEntity<ulong>, IChannel
{
    #region RestChannel

    internal RestChannel(BaseHeyBoxClient client, ulong id)
        : base(client, id)
    {
    }

    #endregion

    #region IChannel

    /// <inheritdoc />
    string IChannel.Name => string.Empty;

    #endregion
}