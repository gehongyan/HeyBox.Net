using System.Text.RegularExpressions;

namespace HeyBox;

/// <summary>
///     提供用于格式化字符的帮助类。
/// </summary>
public static class Format
{
    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <returns></returns>
    public static string Image(string url, string? alternative = null) =>
        $"![{alternative}]({url})";

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="uri"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <returns></returns>
    public static string Image(Uri uri, string? alternative = null) => Image(uri.OriginalString, alternative);

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="attachment"> 图片的附件信息。 </param>
    /// <returns></returns>
    public static string Image(FileAttachment attachment)
    {
        if (attachment.Type is not AttachmentType.Image)
            throw new InvalidOperationException("The attachment is not an image.");
        if (attachment.Uri is null)
            throw new InvalidOperationException("The attachment has not been uploaded yet.");
        return Image(attachment.Uri.OriginalString, attachment.FileName);
    }

    /// <summary>
    ///     移除文本中的 Markdown 格式字符。
    /// </summary>
    /// <param name="text"> 要移除 Markdown 格式字符的文本。 </param>
    /// <returns> 获取移除 Markdown 格式字符后的文本。 </returns>
    /// <remarks>
    ///     此方法不会过多地分析 Markdown 的复杂格式，只会简单地移除 KMarkdown 中的以下字符：<br />
    ///     <c>*</c>、<c>`</c>、<c>~</c>、<c>&gt;</c>、<c>\</c>。
    /// </remarks>
    public static string StripMarkdown(string text) =>
        Regex.Replace(text, @"\*|`|~|>|\\", "", RegexOptions.Compiled);
}
