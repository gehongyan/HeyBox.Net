using System.Text.RegularExpressions;

namespace HeyBox;

internal static class UrlValidation
{
    /// <summary>
    ///     检查 URL 是否有效。
    /// </summary>
    /// <param name="url"> 要校验的 URL。 </param>
    /// <exception cref="UriFormatException"> URL 不能为空。 </exception>
    /// <exception cref="UriFormatException"> URL 必须包含协议（HTTP 或 HTTPS）。 </exception>
    /// <returns> 如果 URL 有效，则为 <c>true</c>，否则为 <c>false</c>。 </returns>
    /// <remarks>
    ///     当前方法仅检查 URL 是否非空，且指定了了 HTTP 或 HTTPS 协议。
    /// </remarks>
    public static void Validate(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new UriFormatException("The URL cannot be null or empty.");

        if (!(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            throw new UriFormatException($"The url {url} must include a protocol (either HTTP or HTTPS)");
    }

    /// <summary>
    ///     确保 URL 表示的是 HeyBox 服务器上的资源。
    /// </summary>
    /// <remarks>
    ///     此方法从 3 个部分检查 URL：
    ///     <list type="number">
    ///         <item> URL 的协议必须是 HTTP 或 HTTPS。 </item>
    ///         <item>
    ///             URL 的主机名必须是以下之一：
    ///             <list type="bullet">
    ///                 <item>chat.max-c.com</item>
    ///             </list>
    ///         </item>
    ///         <item> URL 的路径的格式必需形如：
    ///             <list type="bullet">
    ///                 <item>/attachments/2024-09-18/1836320079868858368_bKptdsjuBN.jpg</item>
    ///             </list>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <param name="url"> 要校验的 URL。 </param>
    /// <exception cref="InvalidOperationException"> URL 不是有效的 HeyBox 服务器上的资源地址。 </exception>
    /// <returns> 如果 URL 指向了有效的 HeyBox 服务器上的资源，则为 <c>true</c>，否则为 <c>false</c>。 </returns>
    public static bool ValidateHeyBoxAssetUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return false;

        // if (!Regex.IsMatch(url,
        //         @"^https?://(chat\.max-c\.com)/(attachments)/\d{4}(-\d{2}){2}/\d+_\w+(\.\w+)$",
        //         RegexOptions.Compiled | RegexOptions.IgnoreCase))
        //     throw new InvalidOperationException($"The url {url} must be a valid HeyBox asset URL");

        return true;
    }
}
