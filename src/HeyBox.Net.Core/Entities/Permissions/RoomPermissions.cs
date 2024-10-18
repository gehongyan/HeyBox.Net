using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个房间的权限集。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct RoomPermissions
{
    /// <summary>
    ///     获取一个空的 <see cref="RoomPermissions"/>，不包含任何权限。
    /// </summary>
    public static readonly RoomPermissions None = new();

    /// <summary>
    ///     获取一个包含所有可以为房间设置的权限的 <see cref="RoomPermissions"/>。
    /// </summary>
    public static readonly RoomPermissions All = new(0b1_1111_1111_1111_1111_1111_1111_1111_1111_1111);

    /// <summary>
    ///     获取此权限集的原始值。
    /// </summary>
    public ulong RawValue { get; }

    /// <summary>
    ///     获取此权限集的相关用户是否是管理员。
    /// </summary>
    public bool Administrator => Permissions.GetValue(RawValue, RoomPermission.Administrator);

    /// <summary>
    ///     获取此权限集的相关用户是否可以查看频道。
    /// </summary>
    public bool ViewChannel => Permissions.GetValue(RawValue, RoomPermission.ViewChannel);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理频道。
    /// </summary>
    public bool ManageChannels => Permissions.GetValue(RawValue, RoomPermission.ManageChannels);

    /// <summary>
    ///     获取此权限集的相关用户是否可以查看审核日志。
    /// </summary>
    public bool ViewAuditLogs => Permissions.GetValue(RawValue, RoomPermission.ViewAuditLogs);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理角色权限。
    /// </summary>
    public bool ManageRoles => Permissions.GetValue(RawValue, RoomPermission.ManageRoles);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理房间。
    /// </summary>
    public bool ManageRoom => Permissions.GetValue(RawValue, RoomPermission.ManageRoom);

    /// <summary>
    ///     获取此权限集的相关用户是否可以创建邀请。
    /// </summary>
    public bool CreateInvites => Permissions.GetValue(RawValue, RoomPermission.CreateInvites);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理邀请。
    /// </summary>
    public bool ManageInvites => Permissions.GetValue(RawValue, RoomPermission.ManageInvites);

    /// <summary>
    ///     获取此权限集的相关用户是否可以修改昵称。
    /// </summary>
    public bool ChangeNickname => Permissions.GetValue(RawValue, RoomPermission.ChangeNickname);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理昵称。
    /// </summary>
    public bool ManageNicknames => Permissions.GetValue(RawValue, RoomPermission.ManageNicknames);

    /// <summary>
    ///     获取此权限集的相关用户是否可以将某人踢出房间。
    /// </summary>
    public bool KickMembers => Permissions.GetValue(RawValue, RoomPermission.KickMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以将某人踢出频道。
    /// </summary>
    public bool KickFromChannel => Permissions.GetValue(RawValue, RoomPermission.KickFromChannel);

    /// <summary>
    ///     获取此权限集的相关用户是否可以将某人加入房间黑名单。
    /// </summary>
    public bool BanMembers => Permissions.GetValue(RawValue, RoomPermission.BanMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以禁言某人。
    /// </summary>
    public bool SilenceMembers => Permissions.GetValue(RawValue, RoomPermission.SilenceMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以发送消息。
    /// </summary>
    public bool SendMessages => Permissions.GetValue(RawValue, RoomPermission.SendMessages);

    /// <summary>
    ///     获取此权限集的相关用户是否可以上传文件。
    /// </summary>
    public bool AttachFiles => Permissions.GetValue(RawValue, RoomPermission.AttachFiles);

    /// <summary>
    ///     获取此权限集的相关用户是否可以 @全体、@在线和所有权限组。
    /// </summary>
    public bool MentionEveryone => Permissions.GetValue(RawValue, RoomPermission.MentionEveryone);

    /// <summary>
    ///     获取此权限集的相关用户是否可以添加表情回应。
    /// </summary>
    public bool AddReactions => Permissions.GetValue(RawValue, RoomPermission.AddReactions);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理消息。
    /// </summary>
    public bool ManageMessages => Permissions.GetValue(RawValue, RoomPermission.ManageMessages);

    /// <summary>
    ///     获取此权限集的相关用户是否可以接收离线消息。
    /// </summary>
    public bool ReceiveOfflineMessages => Permissions.GetValue(RawValue, RoomPermission.ReceiveOfflineMessages);

    /// <summary>
    ///     获取此权限集的相关用户是否可以创建开黑邀约。
    /// </summary>
    public bool CreateTeamUpInvitations => Permissions.GetValue(RawValue, RoomPermission.CreateTeamUpInvitations);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理开黑邀约。
    /// </summary>
    public bool ManageTeamUpInvitations => Permissions.GetValue(RawValue, RoomPermission.ManageTeamUpInvitations);

    /// <summary>
    ///     获取此权限集的相关用户是否可以加入语音频道。
    /// </summary>
    public bool Connect => Permissions.GetValue(RawValue, RoomPermission.Connect);

    /// <summary>
    ///     获取此权限集的相关用户是否可以语音频道内说话。
    /// </summary>
    public bool Speak => Permissions.GetValue(RawValue, RoomPermission.Speak);

    /// <summary>
    ///     获取此权限集的相关用户是否可以允许直接讲话。
    /// </summary>
    public bool UseVoiceActivity => Permissions.GetValue(RawValue, RoomPermission.UseVoiceActivity);

    /// <summary>
    ///     获取此权限集的相关用户是否可以频道静音。
    /// </summary>
    public bool MuteChannels => Permissions.GetValue(RawValue, RoomPermission.MuteChannels);

    /// <summary>
    ///     获取此权限集的相关用户是否可以静音成员。
    /// </summary>
    public bool MuteMembers => Permissions.GetValue(RawValue, RoomPermission.MuteMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以移动成员。
    /// </summary>
    public bool MoveMembers => Permissions.GetValue(RawValue, RoomPermission.MoveMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理语句。
    /// </summary>
    public bool ManageStatements => Permissions.GetValue(RawValue, RoomPermission.ManageStatements);

    /// <summary>
    ///     获取此权限集的相关用户是否可以修改频道内其他人的邀约。
    /// </summary>
    public bool ModifyChannelInvites => Permissions.GetValue(RawValue, RoomPermission.ModifyChannelInvites);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理表情。
    /// </summary>
    public bool ManageEmojisAndStickers => Permissions.GetValue(RawValue, RoomPermission.ManageEmojisAndStickers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理语音包。
    /// </summary>
    public bool ManageSoundPacks => Permissions.GetValue(RawValue, RoomPermission.ManageSoundPacks);

    /// <summary>
    ///     获取此权限集的相关用户是否可以播放语音包。
    /// </summary>
    public bool PlaySoundPacks => Permissions.GetValue(RawValue, RoomPermission.PlaySoundPacks);

    /// <summary>
    ///     获取此权限集的相关用户是否可以播放伴奏。
    /// </summary>
    public bool ShareAudio => Permissions.GetValue(RawValue, RoomPermission.ShareAudio);

    /// <summary>
    ///     获取此权限集的相关用户是否可以共享屏幕。
    /// </summary>
    public bool ShareScreen => Permissions.GetValue(RawValue, RoomPermission.ShareScreen);

    /// <summary>
    ///     获取此权限集的相关用户是否可以发布组队。
    /// </summary>
    public bool PostTeamUps => Permissions.GetValue(RawValue, RoomPermission.PostTeamUps);

    /// <summary>
    ///     获取此权限集的相关用户是否可以使用机器人命令。
    /// </summary>
    public bool UseBotCommands => Permissions.GetValue(RawValue, RoomPermission.UseBotCommands);

    /// <summary>
    ///     使用指定的权限原始值创建一个 <see cref="RoomPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="rawValue"> 权限原始值。 </param>
    public RoomPermissions(ulong rawValue)
    {
        RawValue = rawValue;
    }

    private RoomPermissions(ulong initialValue,
        bool? administrator = null,
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? viewAuditLogs = null,
        bool? manageRoles = null,
        bool? manageRoom = null,
        bool? createInvites = null,
        bool? manageInvites = null,
        bool? changeNickname = null,
        bool? manageNicknames = null,
        bool? kickMembers = null,
        bool? kickFromChannel = null,
        bool? banMembers = null,
        bool? silenceMembers = null,
        bool? sendMessages = null,
        bool? attachFiles = null,
        bool? mentionEveryone = null,
        bool? addReactions = null,
        bool? manageMessages = null,
        bool? receiveOfflineMessages = null,
        bool? createTeamUpInvitations = null,
        bool? manageTeamUpInvitations = null,
        bool? connect = null,
        bool? speak = null,
        bool? useVoiceActivity = null,
        bool? muteChannels = null,
        bool? muteMembers = null,
        bool? moveMembers = null,
        bool? manageStatements = null,
        bool? modifyChannelInvites = null,
        bool? manageEmojisAndStickers = null,
        bool? manageSoundPacks = null,
        bool? playSoundPacks = null,
        bool? shareAudio = null,
        bool? shareScreen = null,
        bool? postTeamUps = null,
        bool? useBotCommands = null
    )
    {
        ulong value = initialValue;

        Permissions.SetValue(ref value, administrator, RoomPermission.Administrator);
        Permissions.SetValue(ref value, viewChannel, RoomPermission.ViewChannel);
        Permissions.SetValue(ref value, manageChannels, RoomPermission.ManageChannels);
        Permissions.SetValue(ref value, viewAuditLogs, RoomPermission.ViewAuditLogs);
        Permissions.SetValue(ref value, manageRoles, RoomPermission.ManageRoles);
        Permissions.SetValue(ref value, manageRoom, RoomPermission.ManageRoom);
        Permissions.SetValue(ref value, createInvites, RoomPermission.CreateInvites);
        Permissions.SetValue(ref value, manageInvites, RoomPermission.ManageInvites);
        Permissions.SetValue(ref value, changeNickname, RoomPermission.ChangeNickname);
        Permissions.SetValue(ref value, manageNicknames, RoomPermission.ManageNicknames);
        Permissions.SetValue(ref value, kickMembers, RoomPermission.KickMembers);
        Permissions.SetValue(ref value, kickFromChannel, RoomPermission.KickFromChannel);
        Permissions.SetValue(ref value, banMembers, RoomPermission.BanMembers);
        Permissions.SetValue(ref value, silenceMembers, RoomPermission.SilenceMembers);
        Permissions.SetValue(ref value, sendMessages, RoomPermission.SendMessages);
        Permissions.SetValue(ref value, attachFiles, RoomPermission.AttachFiles);
        Permissions.SetValue(ref value, mentionEveryone, RoomPermission.MentionEveryone);
        Permissions.SetValue(ref value, addReactions, RoomPermission.AddReactions);
        Permissions.SetValue(ref value, manageMessages, RoomPermission.ManageMessages);
        Permissions.SetValue(ref value, receiveOfflineMessages, RoomPermission.ReceiveOfflineMessages);
        Permissions.SetValue(ref value, createTeamUpInvitations, RoomPermission.CreateTeamUpInvitations);
        Permissions.SetValue(ref value, manageTeamUpInvitations, RoomPermission.ManageTeamUpInvitations);
        Permissions.SetValue(ref value, connect, RoomPermission.Connect);
        Permissions.SetValue(ref value, speak, RoomPermission.Speak);
        Permissions.SetValue(ref value, useVoiceActivity, RoomPermission.UseVoiceActivity);
        Permissions.SetValue(ref value, muteChannels, RoomPermission.MuteChannels);
        Permissions.SetValue(ref value, muteMembers, RoomPermission.MuteMembers);
        Permissions.SetValue(ref value, moveMembers, RoomPermission.MoveMembers);
        Permissions.SetValue(ref value, manageStatements, RoomPermission.ManageStatements);
        Permissions.SetValue(ref value, modifyChannelInvites, RoomPermission.ModifyChannelInvites);
        Permissions.SetValue(ref value, manageEmojisAndStickers, RoomPermission.ManageEmojisAndStickers);
        Permissions.SetValue(ref value, manageSoundPacks, RoomPermission.ManageSoundPacks);
        Permissions.SetValue(ref value, playSoundPacks, RoomPermission.PlaySoundPacks);
        Permissions.SetValue(ref value, shareAudio, RoomPermission.ShareAudio);
        Permissions.SetValue(ref value, shareScreen, RoomPermission.ShareScreen);
        Permissions.SetValue(ref value, postTeamUps, RoomPermission.PostTeamUps);
        Permissions.SetValue(ref value, useBotCommands, RoomPermission.UseBotCommands);

        RawValue = value;
    }

    /// <summary>
    ///     使用指定的权限位信息创建一个 <see cref="RoomPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="administrator"> 管理员。 </param>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="viewAuditLogs"> 查看审核日志。 </param>
    /// <param name="manageRoles"> 管理角色权限。 </param>
    /// <param name="manageRoom"> 管理房间。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="manageInvites"> 管理邀请。 </param>
    /// <param name="changeNickname"> 修改昵称。 </param>
    /// <param name="manageNicknames"> 管理昵称。 </param>
    /// <param name="kickMembers"> 将某人踢出房间。 </param>
    /// <param name="kickFromChannel"> 将某人踢出频道。 </param>
    /// <param name="banMembers"> 将某人加入房间黑名单。 </param>
    /// <param name="silenceMembers"> 禁言某人。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="attachFiles"> 上传文件。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="addReactions"> 添加表情回应。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="receiveOfflineMessages"> 接收离线消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="manageTeamUpInvitations"> 管理开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteChannels"> 频道静音。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="moveMembers"> 移动成员。 </param>
    /// <param name="manageStatements"> 管理语句。 </param>
    /// <param name="modifyChannelInvites"> 修改频道内其他人的邀约。 </param>
    /// <param name="manageEmojisAndStickers"> 管理表情。 </param>
    /// <param name="manageSoundPacks"> 管理语音包。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="postTeamUps"> 发布组队。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    public RoomPermissions(
        bool administrator = false,
        bool viewChannel = false,
        bool manageChannels = false,
        bool viewAuditLogs = false,
        bool manageRoles = false,
        bool manageRoom = false,
        bool createInvites = false,
        bool manageInvites = false,
        bool changeNickname = false,
        bool manageNicknames = false,
        bool kickMembers = false,
        bool kickFromChannel = false,
        bool banMembers = false,
        bool silenceMembers = false,
        bool sendMessages = false,
        bool attachFiles = false,
        bool mentionEveryone = false,
        bool addReactions = false,
        bool manageMessages = false,
        bool receiveOfflineMessages = false,
        bool createTeamUpInvitations = false,
        bool manageTeamUpInvitations = false,
        bool connect = false,
        bool speak = false,
        bool useVoiceActivity = false,
        bool muteChannels = false,
        bool muteMembers = false,
        bool moveMembers = false,
        bool manageStatements = false,
        bool modifyChannelInvites = false,
        bool manageEmojisAndStickers = false,
        bool manageSoundPacks = false,
        bool playSoundPacks = false,
        bool shareAudio = false,
        bool shareScreen = false,
        bool postTeamUps = false,
        bool useBotCommands = false)
        : this(0,
            administrator, viewChannel, manageChannels, viewAuditLogs, manageRoles, manageRoom, createInvites,
            manageInvites, changeNickname, manageNicknames, kickMembers, kickFromChannel, banMembers, silenceMembers,
            sendMessages, attachFiles, mentionEveryone, addReactions, manageMessages, receiveOfflineMessages,
            createTeamUpInvitations, manageTeamUpInvitations, connect, speak, useVoiceActivity, muteChannels,
            muteMembers, moveMembers, manageStatements, modifyChannelInvites, manageEmojisAndStickers, manageSoundPacks,
            playSoundPacks, shareAudio, shareScreen, postTeamUps, useBotCommands)
    {
    }

    /// <summary>
    ///     以当前权限集为基础，更改指定的权限，返回一个 <see cref="RoomPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="administrator"> 管理员。 </param>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="viewAuditLogs"> 查看审核日志。 </param>
    /// <param name="manageRoles"> 管理角色权限。 </param>
    /// <param name="manageRoom"> 管理房间。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="manageInvites"> 管理邀请。 </param>
    /// <param name="changeNickname"> 修改昵称。 </param>
    /// <param name="manageNicknames"> 管理昵称。 </param>
    /// <param name="kickMembers"> 将某人踢出房间。 </param>
    /// <param name="kickFromChannel"> 将某人踢出频道。 </param>
    /// <param name="banMembers"> 将某人加入房间黑名单。 </param>
    /// <param name="silenceMembers"> 禁言某人。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="attachFiles"> 上传文件。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="addReactions"> 添加表情回应。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="receiveOfflineMessages"> 接收离线消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="manageTeamUpInvitations"> 管理开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteChannels"> 频道静音。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="moveMembers"> 移动成员。 </param>
    /// <param name="manageStatements"> 管理语句。 </param>
    /// <param name="modifyChannelInvites"> 修改频道内其他人的邀约。 </param>
    /// <param name="manageEmojisAndStickers"> 管理表情。 </param>
    /// <param name="manageSoundPacks"> 管理语音包。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="postTeamUps"> 发布组队。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    /// <returns> 更改了指定权限的新的权限集。 </returns>
    public RoomPermissions Modify(
        bool? administrator = null,
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? viewAuditLogs = null,
        bool? manageRoles = null,
        bool? manageRoom = null,
        bool? createInvites = null,
        bool? manageInvites = null,
        bool? changeNickname = null,
        bool? manageNicknames = null,
        bool? kickMembers = null,
        bool? kickFromChannel = null,
        bool? banMembers = null,
        bool? silenceMembers = null,
        bool? sendMessages = null,
        bool? attachFiles = null,
        bool? mentionEveryone = null,
        bool? addReactions = null,
        bool? manageMessages = null,
        bool? receiveOfflineMessages = null,
        bool? createTeamUpInvitations = null,
        bool? manageTeamUpInvitations = null,
        bool? connect = null,
        bool? speak = null,
        bool? useVoiceActivity = null,
        bool? muteChannels = null,
        bool? muteMembers = null,
        bool? moveMembers = null,
        bool? manageStatements = null,
        bool? modifyChannelInvites = null,
        bool? manageEmojisAndStickers = null,
        bool? manageSoundPacks = null,
        bool? playSoundPacks = null,
        bool? shareAudio = null,
        bool? shareScreen = null,
        bool? postTeamUps = null,
        bool? useBotCommands = null) =>
        new(RawValue,
            administrator, viewChannel, manageChannels, viewAuditLogs, manageRoles, manageRoom, createInvites,
            manageInvites, changeNickname, manageNicknames, kickMembers, kickFromChannel, banMembers, silenceMembers,
            sendMessages, attachFiles, mentionEveryone, addReactions, manageMessages, receiveOfflineMessages,
            createTeamUpInvitations, manageTeamUpInvitations, connect, speak, useVoiceActivity, muteChannels,
            muteMembers, moveMembers, manageStatements, modifyChannelInvites, manageEmojisAndStickers, manageSoundPacks,
            playSoundPacks, shareAudio, shareScreen, postTeamUps, useBotCommands);

    /// <summary>
    ///     获取当前权限集是否包含指定的权限。
    /// </summary>
    /// <param name="permission"> 要检查的权限。 </param>
    /// <returns> 如果当前权限集包含了所有指定的权限信息，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public bool Has(RoomPermission permission) => Permissions.GetValue(RawValue, permission);

    /// <summary>
    ///     获取一个包含当前权限集所包含的所有已设置的 <see cref="RoomPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限集所包含的所有已设置的 <see cref="RoomPermission"/> 独立位标志枚举值的集合；如果当前权限集未包含任何已设置的权限位，则会返回一个空集合。 </returns>
    public List<RoomPermission> ToList()
    {
        List<RoomPermission> perms = [];

        // bitwise operations on raw value
        // each of the RoomPermissions increments by 2^i from 0 to MaxBits
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            ulong flag = (ulong)1 << i;
            if ((RawValue & flag) != 0)
                perms.Add((RoomPermission)flag);
        }

        return perms;
    }

    internal void Ensure(RoomPermission permissions)
    {
        if (!Has(permissions))
        {
            IEnumerable<RoomPermission> vals = Enum
                .GetValues(typeof(RoomPermission))
                .Cast<RoomPermission>();
            ulong currentValues = RawValue;
            IEnumerable<RoomPermission> missingValues = vals
                .Where(x => permissions.HasFlag(x) && !Permissions.GetValue(currentValues, x))
                .ToList();

            throw new InvalidOperationException(
                $"Missing required room permission{(missingValues.Count() > 1 ? "s" : "")} {string.Join(", ", missingValues.Select(x => x.ToString()))} in order to execute this operation.");
        }
    }

    /// <summary>
    ///     获取此权限集原始值的字符串表示。
    /// </summary>
    /// <returns> 此权限集原始值的字符串表示。 </returns>
    public override string ToString() => RawValue.ToString();

    private string DebuggerDisplay => $"{string.Join(", ", ToList())}";
}
