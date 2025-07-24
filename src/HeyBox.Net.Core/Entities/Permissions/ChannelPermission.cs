namespace HeyBox;

/// <summary>
///     表示可以为角色或用户设置的频道级别的服务器权限。
/// </summary>
[Flags]
public enum ChannelPermission : ulong
{
    /// <summary>
    ///     查看频道
    /// </summary>
    /// <remarks>
    ///     允许成员查看频道（私密频道除外）。
    /// </remarks>
    ViewChannel = 1UL << 1,

    /// <summary>
    ///     管理频道
    /// </summary>
    /// <remarks>
    ///     允许成员创建、编辑、删除频道。
    /// </remarks>
    ManageChannels = 1UL << 2,

    /// <summary>
    ///     创建邀请
    /// </summary>
    /// <remarks>
    ///     允许成员邀请新人加入服务器。
    /// </remarks>
    CreateInvites = 1UL << 6,

    /// <summary>
    ///     将某人踢出频道
    /// </summary>
    /// <remarks>
    ///     允许成员将某人踢出频道。
    /// </remarks>
    KickFromChannel = 1UL << 11,

    /// <summary>
    ///     发送消息
    /// </summary>
    /// <remarks>
    ///     允许成员发送消息。
    /// </remarks>
    SendMessages = 1UL << 14,

    /// <summary>
    ///     上传文件
    /// </summary>
    /// <remarks>
    ///     允许成员上传文件。
    /// </remarks>
    AttachFiles = 1UL << 15,

    /// <summary>
    ///     @全体、@在线和所有权限组
    /// </summary>
    /// <remarks>
    ///     允许成员@全体成员或@在线成员，或@不同权限组的成员。
    /// </remarks>
    MentionEveryone = 1UL << 16,

    /// <summary>
    ///     管理消息
    /// </summary>
    /// <remarks>
    ///     允许成员删除其他成员发出的消息或设置精华消息。
    /// </remarks>
    ManageMessages = 1UL << 18,

    /// <summary>
    ///     创建开黑邀约
    /// </summary>
    /// <remarks>
    ///     允许成员创建开黑邀约。
    /// </remarks>
    CreateTeamUpInvitations = 1UL << 20,

    /// <summary>
    ///     管理开黑邀约
    /// </summary>
    /// <remarks>
    ///     允许成员管理开黑邀约。
    /// </remarks>
    ManageTeamUpInvitations = 1UL << 21,

    /// <summary>
    ///     加入语音频道
    /// </summary>
    /// <remarks>
    ///     允许成员加入语音频道，并听到其他成员发言。
    /// </remarks>
    Connect = 1UL << 22,

    /// <summary>
    ///     语音频道内说话
    /// </summary>
    /// <remarks>
    ///     允许成员在语音频道内说话。如果该权限未开启，则成员默认处于已静音状态，直至具备"静音成员"权限者解除其静音状态为止。
    /// </remarks>
    Speak = 1UL << 23,

    /// <summary>
    ///     允许直接讲话
    /// </summary>
    /// <remarks>
    ///     允许成员通过直接讲话的方式在语音频道内发言。如果该权限未开启，则成员需要使用按键说话的方式进行发言。
    /// </remarks>
    UseVoiceActivity = 1UL << 24,

    /// <summary>
    ///     频道静音
    /// </summary>
    /// <remarks>
    ///     静音频道中所有用户。
    /// </remarks>
    MuteChannels = 1UL << 25,

    /// <summary>
    ///     静音成员
    /// </summary>
    /// <remarks>
    ///     被静音的成员说话将无法被他人听到。
    /// </remarks>
    MuteMembers = 1UL << 26,

    /// <summary>
    ///     移动成员
    /// </summary>
    /// <remarks>
    ///     允许成员断开连接或在语音频道之间移动其他成员。
    /// </remarks>
    MoveMembers = 1UL << 27,

    /// <summary>
    ///     播放语音包
    /// </summary>
    /// <remarks>
    ///     允许成员在房间播放语音包。
    /// </remarks>
    PlaySoundPacks = 1UL << 32,

    /// <summary>
    ///     播放伴奏
    /// </summary>
    /// <remarks>
    ///     允许成员在房间共享伴奏。
    /// </remarks>
    ShareAudio = 1UL << 33,

    /// <summary>
    ///     共享屏幕
    /// </summary>
    /// <remarks>
    ///     允许成员在此房间内进行屏幕共享。
    /// </remarks>
    ShareScreen = 1UL << 34,

    /// <summary>
    ///     使用机器人命令
    /// </summary>
    /// <remarks>
    ///     允许成员使用机器人指令，包括直接输入指令或选择输入框中的“使用机器人指令”。
    /// </remarks>
    UseBotCommands = 1UL << 36,

    /// <summary>
    ///     创建投票
    /// </summary>
    /// <remarks>
    ///     允许成员创建投票。
    /// </remarks>
    CreatePoll = 1UL << 37,

    /// <summary>
    ///     发送频道。
    /// </summary>
    /// <remarks>
    ///     允许成员在频道内发送图片或 Markdown 格式的图片消息。
    /// </remarks>
    SendImages = 1UL << 39,

    /// <summary>
    ///     开启录音。
    /// </summary>
    /// <remarks>
    ///     允许成员在语音频道内开启录音。
    /// </remarks>
    RecordAudio = 1UL << 40,
}
