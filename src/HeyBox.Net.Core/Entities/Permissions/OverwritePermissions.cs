using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一组权限重写配置。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct OverwritePermissions
{
    /// <summary>
    ///     获取一个空的 <see cref="OverwritePermissions"/>，继承所有权限。
    /// </summary>
    public static OverwritePermissions InheritAll { get; } = new();

    /// <summary>
    ///     获取一个在权限重写配置中为指定频道重写允许所有权限的 <see cref="OverwritePermissions"/>。
    /// </summary>
    /// <exception cref="ArgumentException"> 未知的频道类型。 </exception>
    public static OverwritePermissions AllowAll(IChannel channel) =>
        new(ChannelPermissions.All(channel).RawValue, 0);

    /// <summary>
    ///     获取一个在权限重写配置中为指定频道重写禁止所有权限的 <see cref="OverwritePermissions"/>。
    /// </summary>
    /// <exception cref="ArgumentException"> 未知的频道类型。 </exception>
    public static OverwritePermissions DenyAll(IChannel channel) =>
        new(0, ChannelPermissions.All(channel).RawValue);

    /// <summary>
    ///     获取一个表示此重写中所有允许的权限的原始值。
    /// </summary>
    public ulong AllowValue { get; }

    /// <summary>
    ///     获取一个表示此重写中所有禁止的权限的原始值。
    /// </summary>
    public ulong DenyValue { get; }

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ViewChannel"/> 的重写配置。
    /// </summary>
    public PermValue ViewChannel => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ViewChannel);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ManageChannels"/> 的重写配置。
    /// </summary>
    public PermValue ManageChannels => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ManageChannels);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.CreateInvites"/> 的重写配置。
    /// </summary>
    public PermValue CreateInvites => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.CreateInvites);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.KickFromChannel"/> 的重写配置。
    /// </summary>
    public PermValue KickFromChannel => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.KickFromChannel);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.SendMessages"/> 的重写配置。
    /// </summary>
    public PermValue SendMessages => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.SendMessages);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.AttachFiles"/> 的重写配置。
    /// </summary>
    public PermValue AttachFiles => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.AttachFiles);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.MentionEveryone"/> 的重写配置。
    /// </summary>
    public PermValue MentionEveryone => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.MentionEveryone);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ManageMessages"/> 的重写配置。
    /// </summary>
    public PermValue ManageMessages => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ManageMessages);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.CreateTeamUpInvitations"/> 的重写配置。
    /// </summary>
    public PermValue CreateTeamUpInvitations =>
        Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.CreateTeamUpInvitations);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ManageTeamUpInvitations"/> 的重写配置。
    /// </summary>
    public PermValue ManageTeamUpInvitations =>
        Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ManageTeamUpInvitations);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.Connect"/> 的重写配置。
    /// </summary>
    public PermValue Connect => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.Connect);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.Speak"/> 的重写配置。
    /// </summary>
    public PermValue Speak => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.Speak);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.UseVoiceActivity"/> 的重写配置。
    /// </summary>
    public PermValue UseVoiceActivity =>
        Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.UseVoiceActivity);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.MuteChannels"/> 的重写配置。
    /// </summary>
    public PermValue MuteChannels => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.MuteChannels);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.MuteMembers"/> 的重写配置。
    /// </summary>
    public PermValue MuteMembers => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.MuteMembers);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.MoveMembers"/> 的重写配置。
    /// </summary>
    public PermValue MoveMembers => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.MoveMembers);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.PlaySoundPacks"/> 的重写配置。
    /// </summary>
    public PermValue PlaySoundPacks => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.PlaySoundPacks);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ShareAudio"/> 的重写配置。
    /// </summary>
    public PermValue ShareAudio => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ShareAudio);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.ShareScreen"/> 的重写配置。
    /// </summary>
    public PermValue ShareScreen => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ShareScreen);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.UseBotCommands"/> 的重写配置。
    /// </summary>
    public PermValue UseBotCommands => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.UseBotCommands);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.CreatePoll"/> 的重写配置。
    /// </summary>
    public PermValue CreatePoll => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.CreatePoll);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.SendImages"/> 的重写配置。
    /// </summary>
    public PermValue SendImages => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.SendImages);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="HeyBox.ChannelPermission.RecordAudio"/> 的重写配置。
    /// </summary>
    public PermValue RecordAudio => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.RecordAudio);

    /// <summary>
    ///     使用指定的原始值初始化一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="allowValue"> 重写允许的权限的原始值。 </param>
    /// <param name="denyValue"> 重写禁止的权限的原始值。 </param>
    public OverwritePermissions(ulong allowValue, ulong denyValue)
    {
        AllowValue = allowValue;
        DenyValue = denyValue;
    }

    private OverwritePermissions(ulong allowValue, ulong denyValue,
        PermValue? viewChannel = null,
        PermValue? manageChannels = null,
        PermValue? createInvites = null,
        PermValue? kickFromChannel = null,
        PermValue? sendMessages = null,
        PermValue? attachFiles = null,
        PermValue? mentionEveryone = null,
        PermValue? manageMessages = null,
        PermValue? createTeamUpInvitations = null,
        PermValue? manageTeamUpInvitations = null,
        PermValue? connect = null,
        PermValue? speak = null,
        PermValue? useVoiceActivity = null,
        PermValue? muteChannels = null,
        PermValue? muteMembers = null,
        PermValue? moveMembers = null,
        PermValue? playSoundPacks = null,
        PermValue? shareAudio = null,
        PermValue? shareScreen = null,
        PermValue? useBotCommands = null,
        PermValue? createPoll = null,
        PermValue? sendImages = null,
        PermValue? recordAudio = null)
    {
        Permissions.SetValue(ref allowValue, ref denyValue, viewChannel, ChannelPermission.ViewChannel);
        Permissions.SetValue(ref allowValue, ref denyValue, manageChannels, ChannelPermission.ManageChannels);
        Permissions.SetValue(ref allowValue, ref denyValue, createInvites, ChannelPermission.CreateInvites);
        Permissions.SetValue(ref allowValue, ref denyValue, kickFromChannel, ChannelPermission.KickFromChannel);
        Permissions.SetValue(ref allowValue, ref denyValue, sendMessages, ChannelPermission.SendMessages);
        Permissions.SetValue(ref allowValue, ref denyValue, attachFiles, ChannelPermission.AttachFiles);
        Permissions.SetValue(ref allowValue, ref denyValue, mentionEveryone, ChannelPermission.MentionEveryone);
        Permissions.SetValue(ref allowValue, ref denyValue, manageMessages, ChannelPermission.ManageMessages);
        Permissions.SetValue(ref allowValue, ref denyValue, createTeamUpInvitations, ChannelPermission.CreateTeamUpInvitations);
        Permissions.SetValue(ref allowValue, ref denyValue, manageTeamUpInvitations, ChannelPermission.ManageTeamUpInvitations);
        Permissions.SetValue(ref allowValue, ref denyValue, connect, ChannelPermission.Connect);
        Permissions.SetValue(ref allowValue, ref denyValue, speak, ChannelPermission.Speak);
        Permissions.SetValue(ref allowValue, ref denyValue, useVoiceActivity, ChannelPermission.UseVoiceActivity);
        Permissions.SetValue(ref allowValue, ref denyValue, muteChannels, ChannelPermission.MuteChannels);
        Permissions.SetValue(ref allowValue, ref denyValue, muteMembers, ChannelPermission.MuteMembers);
        Permissions.SetValue(ref allowValue, ref denyValue, moveMembers, ChannelPermission.MoveMembers);
        Permissions.SetValue(ref allowValue, ref denyValue, playSoundPacks, ChannelPermission.PlaySoundPacks);
        Permissions.SetValue(ref allowValue, ref denyValue, shareAudio, ChannelPermission.ShareAudio);
        Permissions.SetValue(ref allowValue, ref denyValue, shareScreen, ChannelPermission.ShareScreen);
        Permissions.SetValue(ref allowValue, ref denyValue, useBotCommands, ChannelPermission.UseBotCommands);
        Permissions.SetValue(ref allowValue, ref denyValue, createPoll, ChannelPermission.CreatePoll);
        Permissions.SetValue(ref allowValue, ref denyValue, sendImages, ChannelPermission.SendImages);
        Permissions.SetValue(ref allowValue, ref denyValue, recordAudio, ChannelPermission.RecordAudio);

        AllowValue = allowValue;
        DenyValue = denyValue;
    }

    /// <summary>
    ///     使用指定的权限重写信息创建一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="kickFromChannel"> 将某人踢出频道。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="attachFiles"> 上传文件。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="manageTeamUpInvitations"> 管理开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteChannels"> 频道静音。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="moveMembers"> 移动成员。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    /// <param name="createPoll"> 创建投票。 </param>
    /// <param name="sendImages"> 发送图片或 Markdown 格式的图片消息。 </param>
    /// <param name="recordAudio"> 开启录音。 </param>
    public OverwritePermissions(
        PermValue viewChannel = PermValue.Inherit,
        PermValue manageChannels = PermValue.Inherit,
        PermValue createInvites = PermValue.Inherit,
        PermValue kickFromChannel = PermValue.Inherit,
        PermValue sendMessages = PermValue.Inherit,
        PermValue attachFiles = PermValue.Inherit,
        PermValue mentionEveryone = PermValue.Inherit,
        PermValue manageMessages = PermValue.Inherit,
        PermValue createTeamUpInvitations = PermValue.Inherit,
        PermValue manageTeamUpInvitations = PermValue.Inherit,
        PermValue connect = PermValue.Inherit,
        PermValue speak = PermValue.Inherit,
        PermValue useVoiceActivity = PermValue.Inherit,
        PermValue muteChannels = PermValue.Inherit,
        PermValue muteMembers = PermValue.Inherit,
        PermValue moveMembers = PermValue.Inherit,
        PermValue playSoundPacks = PermValue.Inherit,
        PermValue shareAudio = PermValue.Inherit,
        PermValue shareScreen = PermValue.Inherit,
        PermValue useBotCommands = PermValue.Inherit,
        PermValue createPoll = PermValue.Inherit,
        PermValue sendImages = PermValue.Inherit,
        PermValue recordAudio = PermValue.Inherit)
        : this(0, 0, viewChannel, manageChannels, createInvites, kickFromChannel, sendMessages,
            attachFiles, mentionEveryone, manageMessages, createTeamUpInvitations, manageTeamUpInvitations, connect,
            speak, useVoiceActivity, muteChannels, muteMembers, moveMembers, playSoundPacks, shareAudio, shareScreen,
            useBotCommands, createPoll, sendImages, recordAudio)
    {
    }

    /// <summary>
    ///     以当前权限重写配置为基础，更改指定的重写，返回一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="kickFromChannel"> 将某人踢出频道。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="attachFiles"> 上传文件。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="manageTeamUpInvitations"> 管理开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteChannels"> 频道静音。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="moveMembers"> 移动成员。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    /// <param name="createPoll"> 创建投票。 </param>
    /// <param name="sendImages"> 发送图片或 Markdown 格式的图片消息。 </param>
    /// <param name="recordAudio"> 开启录音。 </param>
    /// <returns> 更改了指定权限的新的权限集。 </returns>
    public OverwritePermissions Modify(
        PermValue? viewChannel = null,
        PermValue? manageChannels = null,
        PermValue? createInvites = null,
        PermValue? kickFromChannel = null,
        PermValue? sendMessages = null,
        PermValue? attachFiles = null,
        PermValue? mentionEveryone = null,
        PermValue? manageMessages = null,
        PermValue? createTeamUpInvitations = null,
        PermValue? manageTeamUpInvitations = null,
        PermValue? connect = null,
        PermValue? speak = null,
        PermValue? useVoiceActivity = null,
        PermValue? muteChannels = null,
        PermValue? muteMembers = null,
        PermValue? moveMembers = null,
        PermValue? playSoundPacks = null,
        PermValue? shareAudio = null,
        PermValue? shareScreen = null,
        PermValue? useBotCommands = null,
        PermValue? createPoll = null,
        PermValue? sendImages = null,
        PermValue? recordAudio = null) =>
        new(AllowValue, DenyValue, viewChannel, manageChannels, createInvites, kickFromChannel, sendMessages,
            attachFiles, mentionEveryone, manageMessages, createTeamUpInvitations, manageTeamUpInvitations, connect,
            speak, useVoiceActivity, muteChannels, muteMembers, moveMembers, playSoundPacks, shareAudio, shareScreen,
            useBotCommands, createPoll, sendImages, recordAudio);

    /// <summary>
    ///     获取一个包含当前权限重写配置所包含的所有重写允许的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限重写配置所包含的所有重写允许的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限重写配置未包含任何重写允许的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToAllowList()
    {
        List<ChannelPermission> perms = [];
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            // first operand must be long or ulong to shift >31 bits
            ulong flag = (ulong)1 << i;
            if ((AllowValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    /// <summary>
    ///     获取一个包含当前权限重写配置所包含的所有重写禁止的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限重写配置所包含的所有重写禁止的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限重写配置未包含任何重写禁止的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToDenyList()
    {
        List<ChannelPermission> perms = new();
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            ulong flag = (ulong)1 << i;
            if ((DenyValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    /// <summary>
    ///     获取此权限重写配置所重写允许与重写禁止的权限的原始值的字符串表示。
    /// </summary>
    /// <returns> 此权限重写配置所重写允许与重写禁止的权限的原始值的字符串表示。 </returns>
    public override string ToString() => $"Allow {AllowValue}, Deny {DenyValue}";

    private string DebuggerDisplay =>
        $"Allow {string.Join(", ", ToAllowList())}, " + $"Deny {string.Join(", ", ToDenyList())}";
}
