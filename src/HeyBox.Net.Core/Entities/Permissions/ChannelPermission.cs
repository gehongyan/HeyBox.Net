namespace HeyBox;

/// <summary>
///     表示可以为角色或用户设置的频道级别的服务器权限。
/// </summary>
[Flags]
public enum ChannelPermission : ulong
{
    /// <inheritdoc cref="HeyBox.RoomPermission.ViewChannel" />
    ViewChannel = 1UL << 1,

    /// <inheritdoc cref="HeyBox.RoomPermission.ManageChannels" />
    ManageChannels = 1UL << 2,

    /// <inheritdoc cref="HeyBox.RoomPermission.CreateInvites" />
    CreateInvites = 1UL << 6,

    /// <inheritdoc cref="HeyBox.RoomPermission.KickFromChannel" />
    KickFromChannel = 1UL << 11,

    /// <inheritdoc cref="HeyBox.RoomPermission.SendMessages" />
    SendMessages = 1UL << 14,

    /// <inheritdoc cref="HeyBox.RoomPermission.AttachFiles" />
    AttachFiles = 1UL << 15,

    /// <inheritdoc cref="HeyBox.RoomPermission.MentionEveryone" />
    MentionEveryone = 1UL << 16,

    /// <inheritdoc cref="HeyBox.RoomPermission.ManageMessages" />
    ManageMessages = 1UL << 18,

    /// <inheritdoc cref="HeyBox.RoomPermission.CreateTeamUpInvitations" />
    CreateTeamUpInvitations = 1UL << 20,

    /// <inheritdoc cref="HeyBox.RoomPermission.ManageTeamUpInvitations" />
    ManageTeamUpInvitations = 1UL << 21,

    /// <inheritdoc cref="HeyBox.RoomPermission.Connect" />
    Connect = 1UL << 22,

    /// <inheritdoc cref="HeyBox.RoomPermission.Speak" />
    Speak = 1UL << 23,

    /// <inheritdoc cref="HeyBox.RoomPermission.UseVoiceActivity" />
    UseVoiceActivity = 1UL << 24,

    /// <inheritdoc cref="HeyBox.RoomPermission.MuteChannels" />
    MuteChannels = 1UL << 25,

    /// <inheritdoc cref="HeyBox.RoomPermission.MuteMembers" />
    MuteMembers = 1UL << 26,

    /// <inheritdoc cref="HeyBox.RoomPermission.MoveMembers" />
    MoveMembers = 1UL << 27,

    /// <inheritdoc cref="HeyBox.RoomPermission.PlaySoundPacks" />
    PlaySoundPacks = 1UL << 32,

    /// <inheritdoc cref="HeyBox.RoomPermission.ShareAudio" />
    ShareAudio = 1UL << 33,

    /// <inheritdoc cref="HeyBox.RoomPermission.ShareScreen" />
    ShareScreen = 1UL << 34,

    /// <inheritdoc cref="HeyBox.RoomPermission.UseBotCommands" />
    UseBotCommands = 1UL << 36,

    /// <inheritdoc cref="HeyBox.RoomPermission.CreatePoll" />
    CreatePoll = 1UL << 37,

    /// <inheritdoc cref="HeyBox.RoomPermission.SendImages" />
    SendImages = 1UL << 39,

    /// <inheritdoc cref="HeyBox.RoomPermission.RecordAudio" />
    RecordAudio = 1UL << 40,
}
