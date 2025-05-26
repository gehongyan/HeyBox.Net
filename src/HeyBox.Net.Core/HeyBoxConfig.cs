using System.Collections.Specialized;
using System.Reflection;

namespace HeyBox;

/// <summary>
///     定义 HeyBox.Net 各种基础行为的配置项。
/// </summary>
public class HeyBoxConfig
{
    /// <summary>
    ///     获取 HeyBox.Net 使用的 API 版本。
    /// </summary>
    public const int APIVersion = 2;

    /// <summary>
    ///     获取 HeyBox.Net 使用的默认请求超时时间。
    /// </summary>
    /// <returns> 一个包含详细版本信息的字符串，包括构建号；当无法获取构建版本时为 <c>Unknown</c>。 </returns>
    public static string Version { get; } =
        typeof(HeyBoxConfig).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(HeyBoxConfig).GetTypeInfo().Assembly.GetName().Version?.ToString(3)
        ?? "Unknown";

    /// <summary>
    ///     获取 HeyBox.Net 在每个请求中使用的用户代理。
    /// </summary>
    public static string UserAgent { get; } = $"HeyBox (https://github.com/gehongyan/HeyBox.Net, v{Version})";

    /// <summary>
    ///     获取黑盒语音 API 请求的根 URL。
    /// </summary>
    public const string APIUrl = "https://chat.xiaoheihe.cn/";

    /// <summary>
    ///     获取黑盒语音上传媒体文件 API 请求的根 URL。
    /// </summary>
    public const string CreateAssetAPIUrl = "https://chat-upload.xiaoheihe.cn/";

    /// <summary>
    ///     获取黑盒语音的版本号。
    /// </summary>
    public const string ChatVersion = "1.29.0";

    /// <summary>
    ///     获取用于 API 请求的公共查询参数。
    /// </summary>
    public static NameValueCollection CommonQueryParameters { get; } = new()
    {
        { "chat_os_type", "bot" },
        { "chat_version", ChatVersion },
        { "client_type", "heybox_chat" },
        { "os_type", "web" },
        { "x_app", "heybox_chat" },
        { "x_client_type", "web" },
        { "x_os_type", "bot" }
    };

    /// <summary>
    ///     获取用于 API 请求的公共查询参数的字符串表示形式。
    /// </summary>
    public static string CommonQueryString { get; } =
        string.Join("&", CommonQueryParameters.AllKeys.Select(k => $"{k}={CommonQueryParameters[k]}"));

    /// <summary>
    ///     获取请求超时的默认时间，以毫秒为单位。
    /// </summary>
    public const int DefaultRequestTimeout = 6000;

    /// <summary>
    ///     获取黑盒语音允许在每个请求中获取的最大房间数量。
    /// </summary>
    public const int MaxRoomsPerBatchByDefault = 20;

    /// <summary>
    ///     获取或设置请求在出现错误时的默认行为。
    /// </summary>
    /// <seealso cref="HeyBox.RequestOptions.RetryMode"/>
    public RetryMode DefaultRetryMode { get; set; } = RetryMode.AlwaysRetry;

    /// <summary>
    ///     获取或设置默认的速率限制回调委托。
    /// </summary>
    /// <remarks>
    ///     若同时设置了此属性与用于各个请求的 <see cref="HeyBox.RequestOptions.RatelimitCallback"/>，则将优先使用
    ///     <see cref="HeyBox.RequestOptions.RatelimitCallback"/>。
    /// </remarks>
    public Func<IRateLimitInfo, Task>? DefaultRatelimitCallback { get; set; }

    /// <summary>
    ///     获取或设置将发送到日志事件的最低日志严重性级别。
    /// </summary>
    public LogSeverity LogLevel { get; set; } = LogSeverity.Info;

    /// <summary>
    ///     获取或设置是否应打印初次启动时要打印的日志。
    /// </summary>
    /// <remarks>
    ///     如果设置为 <c>true</c>，则将在启动时打印库的当前版本，以及所使用的 API 版本。
    /// </remarks>
    internal bool DisplayInitialLog { get; set; } = true;
}
