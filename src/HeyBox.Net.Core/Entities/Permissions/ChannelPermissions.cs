using System.Diagnostics;

namespace HeyBox;

/// <summary>
///     表示一个频道的权限集。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct ChannelPermissions
{
    /// <summary>
    ///     获取一个空的 <see cref="ChannelPermissions"/>，不包含任何权限。
    /// </summary>
    public static readonly ChannelPermissions None = new();

    /// <summary>
    ///     获取一个包含所有可以为文字频道设置的权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    public static readonly ChannelPermissions Text = new(0b1_0000_0000_0000_0001_0101_0100_0000_0100_0110);

    /// <summary>
    ///     获取一个包含所有可以为语音频道设置的权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    public static readonly ChannelPermissions Voice = new(0b1_0111_0000_0101_1101_0101_0100_0000_0100_0110);

    /// <summary>
    ///     获取一个包含所有可以为分组频道设置的权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    public static readonly ChannelPermissions Category = new(0b1_0000_0000_0000_0001_0101_0100_0000_0100_0110);

    /// <summary>
    ///     为指定的频道根据其类型获取一个包含所有权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    /// <param name="channel"> 要获取其包含所有权限的频道。 </param>
    /// <returns> 一个包含所有该频道可以拥有的权限的 <see cref="ChannelPermissions"/>。 </returns>
    /// <exception cref="ArgumentException"> 未知的频道类型。 </exception>
    public static ChannelPermissions All(IChannel channel) =>
        channel switch
        {
            ITextChannel => Text,
            _ => throw new ArgumentException("Unknown channel type.", nameof(channel))
        };

    /// <summary>
    ///     获取此权限集的原始值。
    /// </summary>
    public ulong RawValue { get; }

    /// <summary>
    ///     获取此权限集的相关用户是否可以查看频道。
    /// </summary>
    public bool ViewChannel => Permissions.GetValue(RawValue, ChannelPermission.ViewChannel);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理频道。
    /// </summary>
    public bool ManageChannels => Permissions.GetValue(RawValue, ChannelPermission.ManageChannels);

    /// <summary>
    ///     获取此权限集的相关用户是否可以创建邀请。
    /// </summary>
    public bool CreateInvites => Permissions.GetValue(RawValue, ChannelPermission.CreateInvites);

    /// <summary>
    ///     获取此权限集的相关用户是否可以发送消息。
    /// </summary>
    public bool SendMessages => Permissions.GetValue(RawValue, ChannelPermission.SendMessages);

    /// <summary>
    ///     获取此权限集的相关用户是否可以 @全体、@在线和所有权限组。
    /// </summary>
    public bool MentionEveryone => Permissions.GetValue(RawValue, ChannelPermission.MentionEveryone);

    /// <summary>
    ///     获取此权限集的相关用户是否可以管理消息。
    /// </summary>
    public bool ManageMessages => Permissions.GetValue(RawValue, ChannelPermission.ManageMessages);

    /// <summary>
    ///     获取此权限集的相关用户是否可以创建开黑邀约。
    /// </summary>
    public bool CreateTeamUpInvitations => Permissions.GetValue(RawValue, ChannelPermission.CreateTeamUpInvitations);

    /// <summary>
    ///     获取此权限集的相关用户是否可以加入语音频道。
    /// </summary>
    public bool Connect => Permissions.GetValue(RawValue, ChannelPermission.Connect);

    /// <summary>
    ///     获取此权限集的相关用户是否可以语音频道内说话。
    /// </summary>
    public bool Speak => Permissions.GetValue(RawValue, ChannelPermission.Speak);

    /// <summary>
    ///     获取此权限集的相关用户是否可以允许直接讲话。
    /// </summary>
    public bool UseVoiceActivity => Permissions.GetValue(RawValue, ChannelPermission.UseVoiceActivity);

    /// <summary>
    ///     获取此权限集的相关用户是否可以静音成员。
    /// </summary>
    public bool MuteMembers => Permissions.GetValue(RawValue, ChannelPermission.MuteMembers);

    /// <summary>
    ///     获取此权限集的相关用户是否可以播放语音包。
    /// </summary>
    public bool PlaySoundPacks => Permissions.GetValue(RawValue, ChannelPermission.PlaySoundPacks);

    /// <summary>
    ///     获取此权限集的相关用户是否可以播放伴奏。
    /// </summary>
    public bool ShareAudio => Permissions.GetValue(RawValue, ChannelPermission.ShareAudio);

    /// <summary>
    ///     获取此权限集的相关用户是否可以共享屏幕。
    /// </summary>
    public bool ShareScreen => Permissions.GetValue(RawValue, ChannelPermission.ShareScreen);

    /// <summary>
    ///     获取此权限集的相关用户是否可以使用机器人命令。
    /// </summary>
    public bool UseBotCommands => Permissions.GetValue(RawValue, ChannelPermission.UseBotCommands);

    /// <summary>
    ///     使用指定的权限原始值创建一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="rawValue"> 权限原始值。 </param>
    public ChannelPermissions(ulong rawValue)
    {
        RawValue = rawValue;
    }

    private ChannelPermissions(ulong initialValue,
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? createInvites = null,
        bool? sendMessages = null,
        bool? mentionEveryone = null,
        bool? manageMessages = null,
        bool? createTeamUpInvitations = null,
        bool? connect = null,
        bool? speak = null,
        bool? useVoiceActivity = null,
        bool? muteMembers = null,
        bool? playSoundPacks = null,
        bool? shareAudio = null,
        bool? shareScreen = null,
        bool? useBotCommands = null
    )
    {
        ulong value = initialValue;

        Permissions.SetValue(ref value, viewChannel, ChannelPermission.ViewChannel);
        Permissions.SetValue(ref value, manageChannels, ChannelPermission.ManageChannels);
        Permissions.SetValue(ref value, createInvites, ChannelPermission.CreateInvites);
        Permissions.SetValue(ref value, sendMessages, ChannelPermission.SendMessages);
        Permissions.SetValue(ref value, mentionEveryone, ChannelPermission.MentionEveryone);
        Permissions.SetValue(ref value, manageMessages, ChannelPermission.ManageMessages);
        Permissions.SetValue(ref value, createTeamUpInvitations, ChannelPermission.CreateTeamUpInvitations);
        Permissions.SetValue(ref value, connect, ChannelPermission.Connect);
        Permissions.SetValue(ref value, speak, ChannelPermission.Speak);
        Permissions.SetValue(ref value, useVoiceActivity, ChannelPermission.UseVoiceActivity);
        Permissions.SetValue(ref value, muteMembers, ChannelPermission.MuteMembers);
        Permissions.SetValue(ref value, playSoundPacks, ChannelPermission.PlaySoundPacks);
        Permissions.SetValue(ref value, shareAudio, ChannelPermission.ShareAudio);
        Permissions.SetValue(ref value, shareScreen, ChannelPermission.ShareScreen);
        Permissions.SetValue(ref value, useBotCommands, ChannelPermission.UseBotCommands);

        RawValue = value;
    }

    /// <summary>
    ///     使用指定的权限位信息创建一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    public ChannelPermissions(
        bool viewChannel = false,
        bool manageChannels = false,
        bool createInvites = false,
        bool sendMessages = false,
        bool mentionEveryone = false,
        bool manageMessages = false,
        bool createTeamUpInvitations = false,
        bool connect = false,
        bool speak = false,
        bool useVoiceActivity = false,
        bool muteMembers = false,
        bool playSoundPacks = false,
        bool shareAudio = false,
        bool shareScreen = false,
        bool useBotCommands = false)
        : this(0,
            viewChannel, manageChannels, createInvites, sendMessages, mentionEveryone, manageMessages,
            createTeamUpInvitations, connect, speak, useVoiceActivity, muteMembers, playSoundPacks, shareAudio,
            shareScreen, useBotCommands)
    {
    }

    /// <summary>
    ///     以当前权限集为基础，更改指定的权限，返回一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看频道。 </param>
    /// <param name="manageChannels"> 管理频道。 </param>
    /// <param name="createInvites"> 创建邀请。 </param>
    /// <param name="sendMessages"> 发送消息。 </param>
    /// <param name="mentionEveryone"> @全体、@在线和所有权限组。 </param>
    /// <param name="manageMessages"> 管理消息。 </param>
    /// <param name="createTeamUpInvitations"> 创建开黑邀约。 </param>
    /// <param name="connect"> 加入语音频道。 </param>
    /// <param name="speak"> 语音频道内说话。 </param>
    /// <param name="useVoiceActivity"> 允许直接讲话。 </param>
    /// <param name="muteMembers"> 静音成员。 </param>
    /// <param name="playSoundPacks"> 播放语音包。 </param>
    /// <param name="shareAudio"> 播放伴奏。 </param>
    /// <param name="shareScreen"> 共享屏幕。 </param>
    /// <param name="useBotCommands"> 使用机器人命令。 </param>
    /// <returns> 更改了指定权限的新的权限集。 </returns>
    public ChannelPermissions Modify(
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? createInvites = null,
        bool? sendMessages = null,
        bool? mentionEveryone = null,
        bool? manageMessages = null,
        bool? createTeamUpInvitations = null,
        bool? connect = null,
        bool? speak = null,
        bool? useVoiceActivity = null,
        bool? muteMembers = null,
        bool? playSoundPacks = null,
        bool? shareAudio = null,
        bool? shareScreen = null,
        bool? useBotCommands = null) =>
        new(RawValue,
            viewChannel, manageChannels, createInvites, sendMessages, mentionEveryone, manageMessages,
            createTeamUpInvitations, connect, speak, useVoiceActivity, muteMembers, playSoundPacks, shareAudio,
            shareScreen, useBotCommands);

    /// <summary>
    ///     获取当前权限集是否包含指定的权限。
    /// </summary>
    /// <param name="permission"> 要检查的权限。 </param>
    /// <returns> 如果当前权限集包含了所有指定的权限信息，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public bool Has(ChannelPermission permission) => Permissions.GetValue(RawValue, permission);

    /// <summary>
    ///     获取一个包含当前权限集所包含的所有已设置的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限集所包含的所有已设置的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限集未包含任何已设置的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToList()
    {
        List<ChannelPermission> perms = [];

        // bitwise operations on raw value
        // each of the ChannelPermissions increments by 2^i from 0 to MaxBits
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            ulong flag = (ulong)1 << i;
            if ((RawValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    internal void Ensure(ChannelPermission permissions)
    {
        if (!Has(permissions))
        {
            IEnumerable<ChannelPermission> vals = Enum
                .GetValues(typeof(ChannelPermission))
                .Cast<ChannelPermission>();
            ulong currentValues = RawValue;
            IEnumerable<ChannelPermission> missingValues = vals
                .Where(x => permissions.HasFlag(x) && !Permissions.GetValue(currentValues, x))
                .ToList();

            throw new InvalidOperationException(
                $"Missing required channel permission{(missingValues.Count() > 1 ? "s" : "")} {string.Join(", ", missingValues.Select(x => x.ToString()))} in order to execute this operation.");
        }
    }

    /// <summary>
    ///     获取此权限集原始值的字符串表示。
    /// </summary>
    /// <returns> 此权限集原始值的字符串表示。 </returns>
    public override string ToString() => RawValue.ToString();

    private string DebuggerDisplay => $"{string.Join(", ", ToList())}";
}
