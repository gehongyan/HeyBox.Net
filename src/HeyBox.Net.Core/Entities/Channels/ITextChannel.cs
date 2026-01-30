namespace HeyBox;

/// <summary>
///     表示房间中一个通用的具有文字聊天能力的频道，可以发送和接收消息。
/// </summary>
public interface ITextChannel : INestedChannel, IMentionable, IMessageChannel;

/// <summary>
///     表示服务器中的一个通用的具有语音聊天能力的频道。
/// </summary>
public interface IVoiceChannel : INestedChannel, IAudioChannel
{
    /// <summary>
    ///     获取用于与该频道进行语音通信的 SDK 提供者。
    /// </summary>
    VoiceChannelSdkProvider SdkProvider { get; }

    /// <summary>
    ///     获取允许同时连接到此频道的最大用户数。
    /// </summary>
    /// <returns> 一个 <c>int</c>，表示允许同时连接到此频道的最大用户数；如果没有限制，则为 <c>0</c>。 </returns>
    int UserLimit { get; }

    /// <summary>
    ///     获取此频道是否已被密码锁定。
    /// </summary>
    bool HasPassword { get; }


}


/// <summary>
///     表示一个通用的音频频道。
/// </summary>
public interface IAudioChannel : IChannel;
