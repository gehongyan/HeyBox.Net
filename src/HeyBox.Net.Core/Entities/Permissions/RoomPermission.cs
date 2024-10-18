namespace HeyBox;

/// <summary>
///     表示可以为角色或用户设置的房间级别的服务器权限。
/// </summary>
[Flags]
public enum RoomPermission : ulong
{
    /// <summary>
    ///     管理员
    /// </summary>
    /// <remarks>
    ///     拥有除删除服务器之外的所有权限。
    /// </remarks>
    Administrator = 1UL << 0,

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
    ///     查看审核日志
    /// </summary>
    /// <remarks>
    ///     允许成员在此房间内查看用户在房间内的操作。
    /// </remarks>
    ViewAuditLogs = 1UL << 3,

    /// <summary>
    ///     管理角色权限
    /// </summary>
    /// <remarks>
    ///     允许成员创建新的权限，可以编辑或删除比自身最高身份组限低的身份组。同时允许成员编辑自身可访问的频道的权限。
    /// </remarks>
    ManageRoles = 1UL << 4,

    /// <summary>
    ///     管理房间
    /// </summary>
    /// <remarks>
    ///     允许成员编辑房间名称，查看所有邀请。
    /// </remarks>
    ManageRoom = 1UL << 5,

    /// <summary>
    ///     创建邀请
    /// </summary>
    /// <remarks>
    ///     允许成员邀请新人加入服务器。
    /// </remarks>
    CreateInvites = 1UL << 6,

    /// <summary>
    ///     管理邀请
    /// </summary>
    /// <remarks>
    ///     允许成员管理邀请。
    /// </remarks>
    ManageInvites = 1UL << 7,

    /// <summary>
    ///     修改昵称
    /// </summary>
    /// <remarks>
    ///     允许用户修改自己在房间的昵称。
    /// </remarks>
    ChangeNickname = 1UL << 8,

    /// <summary>
    ///     管理昵称
    /// </summary>
    /// <remarks>
    ///     允许用户修改别人在房间的昵称。
    /// </remarks>
    ManageNicknames = 1UL << 9,

    /// <summary>
    ///     将某人踢出房间
    /// </summary>
    /// <remarks>
    ///     允许成员将其他成员从此房间踢出。被踢出的成员如果获得新的邀请，可再次加入此房间。
    /// </remarks>
    KickMembers = 1UL << 10,

    /// <summary>
    ///     将某人踢出频道
    /// </summary>
    /// <remarks>
    ///     允许成员将某人踢出频道。
    /// </remarks>
    KickFromChannel = 1UL << 11,

    /// <summary>
    ///     将某人加入房间黑名单
    /// </summary>
    /// <remarks>
    ///     允许用户将其他用户加入房间黑名单，被加入黑名单的用户将永远无法加入此房间，直至被移出黑名单。
    /// </remarks>
    BanMembers = 1UL << 12,

    /// <summary>
    ///     禁言某人
    /// </summary>
    /// <remarks>
    ///     允许用户对其他用户禁言，被禁言的用户无法在文字频道或语音频道中发言。
    /// </remarks>
    SilenceMembers = 1UL << 13,

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
    ///     添加表情回应
    /// </summary>
    /// <remarks>
    ///     允许成员添加表情回应。
    /// </remarks>
    AddReactions = 1UL << 17,

    /// <summary>
    ///     管理消息
    /// </summary>
    /// <remarks>
    ///     允许成员删除其他成员发出的消息或设置精华消息。
    /// </remarks>
    ManageMessages = 1UL << 18,

    /// <summary>
    ///     接收离线消息
    /// </summary>
    /// <remarks>
    ///     允许成员接收离线时房间内的消息。如果关闭此权限，成员只能接收在线时房间内的消息。
    /// </remarks>
    ReceiveOfflineMessages = 1UL << 19,

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
    ///     管理语句
    /// </summary>
    /// <remarks>
    ///     管理语句。
    /// </remarks>
    ManageStatements = 1UL << 28,

    /// <summary>
    ///     修改频道内其他人的邀约
    /// </summary>
    /// <remarks>
    ///     管理频道内其他人的邀约。
    /// </remarks>
    ModifyChannelInvites = 1UL << 29,

    /// <summary>
    ///     管理表情
    /// </summary>
    /// <remarks>
    ///     允许成员在此房间内上传、编辑、移除 Emoji 和大表情。
    /// </remarks>
    ManageEmojisAndStickers = 1UL << 30,

    /// <summary>
    ///     管理语音包
    /// </summary>
    /// <remarks>
    ///     允许成员在此房间内上传、编辑、移除语音包。
    /// </remarks>
    ManageSoundPacks = 1UL << 31,

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
    ShareAudio = 1L << 33,

    /// <summary>
    ///     共享屏幕
    /// </summary>
    /// <remarks>
    ///     允许成员在此房间内进行屏幕共享。
    /// </remarks>
    ShareScreen = 1L << 34,

    /// <summary>
    ///     发布组队
    /// </summary>
    /// <remarks>
    ///     允许成员发布组队。
    /// </remarks>
    PostTeamUps = 1L << 35,

    /// <summary>
    ///     使用机器人命令
    /// </summary>
    /// <remarks>
    ///     允许成员使用机器人指令，包括直接输入指令或选择输入框中的“使用机器人指令”。
    /// </remarks>
    UseBotCommands = 1L << 36
}
