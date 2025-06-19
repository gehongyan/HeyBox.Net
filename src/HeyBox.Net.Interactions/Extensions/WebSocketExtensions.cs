namespace HeyBox.WebSocket;

internal static class WebSocketExtensions
{
    /// <summary>
    ///     获取被执行命令及其父级的名称（按层级顺序）。
    /// </summary>
    /// <param name="data">斜线命令交互数据。</param>
    /// <returns>
    ///     被执行命令及其父级的名称（按层级顺序）。
    /// </returns>
    public static IList<string> GetCommandKeywords(this ISlashCommandInteractionData data) => [data.Name];
}
