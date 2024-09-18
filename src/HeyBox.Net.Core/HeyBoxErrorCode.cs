namespace HeyBox;

/// <summary>
///     表示从 HeyBox 接收到的错误代码。
/// </summary>
public readonly record struct HeyBoxErrorCode
{
    /// <summary>
    ///     获取表示操作成功的代码。
    /// </summary>
    public static readonly HeyBoxErrorCode OK = new("ok");

    internal HeyBoxErrorCode(string rawCode)
    {
        RawCode = rawCode;
    }

    /// <summary>
    ///     获取错误代码。
    /// </summary>
    public string RawCode { get; init; }
}
