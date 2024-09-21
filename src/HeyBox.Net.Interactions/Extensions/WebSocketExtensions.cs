namespace HeyBox.WebSocket;

internal static class WebSocketExtensions
{
    /// <summary>
    ///     Get the name of the executed command and its parents in hierarchical order.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>
    ///     The name of the executed command and its parents in hierarchical order.
    /// </returns>
    public static IList<string> GetCommandKeywords(this ISlashCommandInteractionData data) => [data.Name];
}
