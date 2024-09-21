using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace HeyBox;

/// <summary>
///     提供一组用于处理黑盒语音登录令牌的辅助方法。
/// </summary>
public static class TokenUtils
{
    /// <summary>
    ///     Bot 令牌的最小长度。
    /// </summary>
    /// <remarks>
    ///     此值是通过与文档和现有令牌的示例进行比较确定的。
    /// </remarks>
    internal const int MinBotTokenLength = 64;

    /// <summary>
    ///     Bot 令牌的标准长度。
    /// </summary>
    /// <remarks>
    ///     此值是通过与 HeyBox 文档和现有令牌的示例进行比较确定的。
    /// </remarks>
    internal const int StandardBotTokenLength = 64;

    /// <summary>
    ///     Base64 编码中使用的填充字符。
    /// </summary>
    internal const char Base64Padding = '=';

    /// <summary>
    ///     如果一个 Base64 编码的字符串长度不是 4 的倍数，则使用 0、1 或 2 个 '=' 字符对其进行填充。
    ///     此方法不保证提供的字符串仅包含有效的 Base64 字符，已经包含填充的字符串不会再添加额外的填充字符。
    /// </summary>
    /// <remarks>
    ///     需要 3 个填充字符的 base64 字符串会被视为其格式不正确。
    /// </remarks>
    /// <param name="encodedBase64"> 要用字符填充的 base64 编码字符串。 </param>
    /// <returns> 包含 base64 填充字符的字符串。 </returns>
    /// <exception cref="FormatException">
    ///     如果 <paramref name="encodedBase64"/> 需要无效数量的填充字符，将引发异常。
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     如果 <paramref name="encodedBase64"/> 为 null、空字符串或仅包含空白字符，将引发异常。
    /// </exception>
    internal static string PadBase64String(string encodedBase64)
    {
        if (string.IsNullOrWhiteSpace(encodedBase64))
            throw new ArgumentNullException(encodedBase64,
                "The supplied base64-encoded string was null or whitespace.");

        // do not pad if already contains padding characters
        if (encodedBase64.IndexOf(Base64Padding) != -1)
            return encodedBase64;

        // based from https://stackoverflow.com/a/1228744
        int padding = (4 - encodedBase64.Length % 4) % 4;
        if (padding == 3)
            // can never have 3 characters of padding
            throw new FormatException("The provided base64 string is corrupt, as it requires an invalid amount of padding.");
        if (padding == 0)
            return encodedBase64;
        return encodedBase64.PadRight(encodedBase64.Length + padding, Base64Padding);
    }

    internal static string? DecodeBase64(string encoded)
    {
        if (string.IsNullOrWhiteSpace(encoded)) return null;

        try
        {
            // re-add base64 padding if missing
            encoded = PadBase64String(encoded);
            // decode the base64 string
            byte[] bytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (DecoderFallbackException)
        {
            // ignore exception, can be thrown by GetString
        }
        catch (FormatException)
        {
            // ignore exception, can be thrown if base64 string is invalid
        }
        catch (ArgumentException)
        {
            // ignore exception, can be thrown by BitConverter, or by PadBase64String
        }

        return null;
    }

    internal static bool TryParseBotTokenUserId([NotNullWhen(true)] string? token, out uint userId)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            userId = 0;
            return false;
        }

        // return true if the user id could be determined
        string? plainText = DecodeBase64(token);
        if (plainText == null)
        {
            userId = 0;
            return false;
        }

        // split each component
        string[] segments = plainText.Split(';');

        // ensure that there are three parts
        if (segments.Length != 3)
        {
            userId = 0;
            return false;
        }

        // ensure all parts could be parsed as a ulong
        if (!uint.TryParse(segments[0], NumberStyles.None, CultureInfo.InvariantCulture, out userId))
        {
            userId = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    ///     校验 Bot 令牌的有效性。
    /// </summary>
    /// <param name="token"> 要校验的 Bot 令牌。 </param>
    /// <returns> 如果校验成功，则返回 <c>true</c>；否则返回 <c>false</c>。 </returns>
    internal static bool CheckBotTokenValidity(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;

        // return true if the user id could be determined
        string? plainText = DecodeBase64(token);
        if (plainText == null) return false;

        // split each component
        string[] segments = plainText.Split(';');

        // ensure that there are three parts
        if (segments.Length != 3) return false;

        // ensure all parts could be parsed as a ulong
        if (!ulong.TryParse(segments[0], NumberStyles.None, CultureInfo.InvariantCulture, out ulong userId)) return false;
        if (!ulong.TryParse(segments[1], NumberStyles.None, CultureInfo.InvariantCulture, out ulong timestamp)) return false;
        if (!ulong.TryParse(segments[2], NumberStyles.None, CultureInfo.InvariantCulture, out _)) return false;

        return true;
    }

    /// <summary>
    ///     令牌中不允许的所有字符。
    /// </summary>
    internal static readonly char[] IllegalTokenCharacters = [' ', '\t', '\r', '\n'];

    /// <summary>
    ///     检查给定的令牌是否包含会导致登录失败的空格或换行符。
    /// </summary>
    /// <param name="token"> 要检查的令牌。 </param>
    /// <returns> 如果令牌包含空格或换行符，则返回 <c>true</c>；否则返回 <c>false</c>。 </returns>
    internal static bool CheckContainsIllegalCharacters(string token) =>
        token.IndexOfAny(IllegalTokenCharacters) != -1;

    /// <summary>
    ///     检查指定类型的令牌的有效性。
    /// </summary>
    /// <param name="tokenType"> 令牌的类型。 </param>
    /// <param name="token"> 要校验的令牌。 </param>
    /// <exception cref="ArgumentNullException"> 当提供的令牌值为 <c>null</c>、空字符串或仅包含空白字符时引发异常。 </exception>
    /// <exception cref="ArgumentException"> 当提供的令牌类型或令牌值无效时引发异常。 </exception>
    public static void ValidateToken(TokenType tokenType, string token)
    {
        // A Null or WhiteSpace token of any type is invalid.
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token), "A token cannot be null, empty, or contain only whitespace.");

        // ensure that there are no whitespace or newline characters
        if (CheckContainsIllegalCharacters(token))
            throw new ArgumentException("The token contains a whitespace or newline character. Ensure that the token has been properly trimmed.",
                nameof(token));

        switch (tokenType)
        {
            case TokenType.BotToken:
                // bot tokens are assumed to be at least 64 characters in length
                // this value was determined by referencing examples in the HeyBox documentation, and by comparing with
                // pre-existing tokens
                if (token.Length < MinBotTokenLength || token.TrimEnd('=').Length > StandardBotTokenLength)
                    throw new ArgumentException(
                        $"A Bot token must be at least {MinBotTokenLength} characters in length. "
                        + $"After the ending equal characters are trimmed, any Bot token should not be longer than {StandardBotTokenLength}. ",
                        nameof(token));

                // check the validity of the bot token by decoding the ulong userid
                if (!CheckBotTokenValidity(token))
                    throw new ArgumentException("The Bot token was invalid.",
                        nameof(token));

                break;
            default:
                // All unrecognized TokenTypes (including User tokens) are considered to be invalid.
                throw new ArgumentException("Unrecognized TokenType.", nameof(token));
        }
    }
}