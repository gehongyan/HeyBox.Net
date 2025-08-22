namespace HeyBox;

/// <summary>
///     表示频道语音 SDK 提供者。
/// </summary>
public enum VoiceChannelSdkProvider
{
    /// <summary>
    ///     表示未知的语音 SDK 提供者。
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     腾讯云语音 SDK 提供者。
    /// </summary>
    Tencent = 1,

    /// <summary>
    ///     火山引擎语音 SDK 提供者。
    /// </summary>
    Volcengine = 2
}
