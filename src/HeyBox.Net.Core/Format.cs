using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace HeyBox;

/// <summary>
///     提供用于格式化字符的帮助类。
/// </summary>
public static class Format
{
    // Characters which need escaping
    private static readonly string[] SensitiveCharacters =
    [
        "\\", "*", "_", "~", "`", ".", ":", "/", ">", "|", "#"
    ];

    /// <summary>
    ///     返回一个使用粗体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string Bold(string? text, bool sanitize = true) =>
        $"**{(sanitize ? Sanitize(text, "*") : text)}**";

    /// <summary>
    ///     返回一个使用斜体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string Italics(string? text, bool sanitize = true) =>
        $"*{(sanitize ? Sanitize(text, "*") : text)}*";

    /// <summary>
    ///     返回一个使用粗斜体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string BoldItalics(string? text, bool sanitize = true) =>
        $"***{(sanitize ? Sanitize(text, "*") : text)}***";

    /// <summary>
    ///     返回一个使用删除线格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>~</c> 字符转义为 <c>\~</c>。
    /// </remarks>
    public static string Strikethrough(string? text, bool sanitize = true) =>
        $"~~{(sanitize ? Sanitize(text, "~") : text)}~~";

    /// <summary>
    ///     返回格式化为 Markdown 链接的字符串。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的链接文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c>，将会对 URL 中出现的所有 <c>&lt;</c> 和 <c>&gt;</c> 字符分别转义为
    ///     <c>\&lt;</c> 和 <c>\&gt;</c>。
    /// </remarks>
    public static string Url(string url, bool sanitize = true) => $"<{Sanitize(url, "<", ">")}>";

    /// <inheritdoc cref="Format.Url(System.String,System.Boolean)" />
    public static string Url(Uri url, bool sanitize = true) => Url(url.OriginalString, sanitize);

    /// <summary>
    ///     返回格式化为 Markdown 链接的字符串。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 与 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的链接文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c>，将会对文本中出现的所有 <c>[</c> 和 <c>]</c> 字符分别转义为
    ///     <c>\[</c> 和 <c>\]</c>，并对 URL 中出现的所有 <c>(</c> 和 <c>)</c> 字符分别转义为 <c>\(</c> 和 <c>\)</c>。
    /// </remarks>
    public static string Url(string url, string text, bool sanitize = true) =>
        $"[{(sanitize ? Sanitize(text, "[", "]") : text)}]({(sanitize ? Sanitize(url, "(", ")") : url)})";

    /// <inheritdoc cref="Format.Url(System.String,System.String,System.Boolean)" />
    public static string Url(Uri url, string text, bool sanitize = true) => Url(url.OriginalString, text, sanitize);

    /// <summary>
    ///     返回格式化为 Markdown 一级标题的字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    public static string H1(string text) => $"# {text}";

    /// <summary>
    ///     返回格式化为 Markdown 二级标题的字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    public static string H2(string text) => $"## {text}";

    /// <summary>
    ///     返回格式化为 Markdown 三级标题的字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    public static string H3(string text) => $"### {text}";

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <returns></returns>
    public static string Image(string url, string? alternative = null) => $"![{alternative}]({url})";

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
        return Image(attachment.Uri.OriginalString, attachment.Filename);
    }

    /// <summary>
    ///     获取无序列表的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="items"> 要格式化的列表项。 </param>
    /// <param name="indentLevel"> 列表项的缩进级别。 </param>
    /// <returns> 获取格式化后的列表。 </returns>
    public static string UnorderedList(IEnumerable<string> items, int indentLevel = 0) => items
        .Select(item => $"{new string(' ', indentLevel * 4)}- {item}")
        .Aggregate((current, next) => $"{current}\n{next}");

    /// <summary>
    ///     返回一个使用代码格式的 KMarkdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="language"> 代码块的语言。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的内联代码或代码块。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>`</c> 字符转义为 <c>\`</c>。 <br />
    ///     当 <paramref name="language"/> 不为 <c>null</c> 或 <paramref name="text"/> 中包含换行符时，将返回一个代码块；
    ///     否则，将返回一个内联代码块。
    /// </remarks>
    public static string Code(string? text, string? language = null, bool sanitize = true)
    {
        if (text is null)
            return "``";
        return text.Contains('\n') || string.IsNullOrWhiteSpace(language)
            ? CodeBlock(text, language, sanitize)
            : $"`{(sanitize ? Sanitize(text, "`") : text)}`";
    }

    /// <summary>
    ///     返回一个使用代码块格式的 KMarkdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="language"> 代码块的语言。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的代码块。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>`</c> 字符转义为 <c>\`</c>。
    /// </remarks>
    public static string CodeBlock(string? text, string? language = null, bool sanitize = true) =>
        text is null
            ? "```\n```"
            : $"""
            ```{(language is not null ? language.Trim() : string.Empty)}
            {(sanitize ? Sanitize(text, "`") : text)}
            ```
            """;

    /// <summary>
    ///     转义字符串，安全地转义任何 Markdown 序列。
    /// </summary>
    /// <param name="text"> 要转义的文本。 </param>
    /// <param name="sensitiveCharacters"> 要转义的字符。 </param>
    /// <returns> 获取转义后的文本。 </returns>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? Sanitize(string? text, params string[] sensitiveCharacters)
    {
        if (text is null) return null;
        string[] sensitiveChars = sensitiveCharacters.Length > 0 ? sensitiveCharacters : SensitiveCharacters;
        return sensitiveChars.Aggregate(text,
            (current, unsafeChar) => current.Replace(unsafeChar, $"\\{unsafeChar}"));
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
