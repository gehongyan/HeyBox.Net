using System.Globalization;

namespace HeyBox.Net;

/// <summary>
///     表示一个基于 REST 的速率限制信息。
/// </summary>
public struct RateLimitInfo : IRateLimitInfo
{
    /// <inheritdoc/>
    public bool IsGlobal { get; }

    /// <inheritdoc/>
    public int? Limit { get; }

    /// <inheritdoc/>
    public int? Remaining { get; }

    /// <inheritdoc/>
    public DateTimeOffset? Reset { get; }

    /// <inheritdoc/>
    public TimeSpan? ResetAfter { get; }

    /// <inheritdoc/>
    public string? Bucket { get; }

    /// <inheritdoc/>
    public TimeSpan? Lag { get; }

    /// <inheritdoc/>
    public string Endpoint { get; }

    internal RateLimitInfo(Dictionary<string, string?> headers, string endpoint)
    {
        Endpoint = endpoint;

        IsGlobal = headers.TryGetValue("X-Ratelimit-Bucket", out string? temp)
            && string.IsNullOrWhiteSpace(temp);
        Limit = headers.TryGetValue("X-Ratelimit-Limit", out temp)
            && int.TryParse(temp, NumberStyles.None, CultureInfo.InvariantCulture, out int limit)
                ? limit
                : null;
        Remaining = headers.TryGetValue("X-Ratelimit-Remaining", out temp)
            && int.TryParse(temp, NumberStyles.None, CultureInfo.InvariantCulture, out int remaining)
                ? remaining
                : null;
        // RetryAfter = headers.TryGetValue("Retry-After", out temp) &&
        //              int.TryParse(temp, NumberStyles.None, CultureInfo.InvariantCulture, out var retryAfter) ? retryAfter : (int?)null;
        ResetAfter = headers.TryGetValue("X-Ratelimit-Reset-After", out temp)
            && double.TryParse(temp, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double resetAfter)
                ? TimeSpan.FromSeconds(resetAfter)
                : null;
        Reset = headers.TryGetValue("X-Ratelimit-Reset", out temp)
            && double.TryParse(temp, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var reset)
            && reset != 0
                ? DateTimeOffset.FromUnixTimeSeconds((long)reset)
                : null;
        Bucket = headers.TryGetValue("X-Ratelimit-Bucket", out temp)
            && !string.IsNullOrWhiteSpace(temp)
                ? temp
                : null;
        Lag = headers.TryGetValue("Date", out temp)
            && DateTimeOffset.TryParse(temp, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset date)
                ? DateTimeOffset.UtcNow - date
                : null;
    }
}
