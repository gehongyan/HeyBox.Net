using System.Collections.Immutable;
using System.Net;

namespace HeyBox.Net;

/// <summary>
///     当处理 HeyBox HTTP 请求时发生错误时引发的异常。
/// </summary>
public class HttpException : Exception
{
    /// <summary>
    ///     获取 HeyBox 返回的 HTTP 状态码。
    /// </summary>
    public HttpStatusCode HttpCode { get; }

    /// <summary>
    ///     获取由 HeyBox 返回的 JSON 负载中的错误代码；也有可能是表示操作成功的代码；
    ///     如果无法从响应中解析出错误代码，则为 <see langword="null"/>。
    /// </summary>
    public HeyBoxErrorCode? HeyBoxCode { get; }

    /// <summary>
    ///     获取异常的原因；也有可能是表示操作成功的消息；如果无法从响应中解析出原因，则为 <see langword="null"/>。
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    ///     获取错误代码。
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    ///     获取用于发送请求的请求对象。
    /// </summary>
    public IRequest Request { get; }

    /// <summary>
    ///     获取描述请求失败原因的所有 JSON 错误。
    /// </summary>
    public IReadOnlyCollection<HeyBoxJsonError> Errors { get; }

    /// <summary>
    ///     初始化一个 <see cref="HttpException"/> 类的新实例。
    /// </summary>
    /// <param name="httpCode"> 返回的 HTTP 状态码。 </param>
    /// <param name="request"> 引发异常前发送的请求。 </param>
    /// <param name="heyBoxCode"> 由 HeyBox 返回的 JSON 负载中解析出的状态码。 </param>
    /// <param name="reason"> 引发异常的原因。 </param>
    /// <param name="errors"> 描述请求错误的所有 JSON 错误。 </param>
    public HttpException(HttpStatusCode httpCode, IRequest request,
        HeyBoxErrorCode? heyBoxCode = null, string? reason = null, HeyBoxJsonError[]? errors = null)
        : base(CreateMessage(httpCode, heyBoxCode?.RawCode, reason))
    {
        HttpCode = httpCode;
        Request = request;
        HeyBoxCode = heyBoxCode;
        Reason = reason;
        ErrorCode = heyBoxCode?.RawCode;
        Errors = errors?.ToImmutableArray() ?? [];
    }

    private static string CreateMessage(HttpStatusCode httpCode, string? heyBoxCode = null, string? reason = null)
    {
        string closeCode = !string.IsNullOrWhiteSpace(heyBoxCode)
            ? heyBoxCode
            : httpCode.ToString();
        return reason != null
            ? $"The server responded with error {closeCode}: {reason}"
            : $"The server responded with error {closeCode}: {httpCode}";
    }
}
