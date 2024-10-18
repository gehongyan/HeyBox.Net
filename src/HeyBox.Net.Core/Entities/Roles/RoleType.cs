namespace HeyBox;

/// <summary>
///     表示角色的类型。
/// </summary>
public enum RoleType : ushort
{
    /// <summary>
    ///     自定义角色
    /// </summary>
    Custom = 0,

    /// <summary>
    ///     游戏角色
    /// </summary>
    Game = 1,

    /// <summary>
    ///     机器人
    /// </summary>
    Bot = 2,

    /// <summary>
    ///     成员管理员
    /// </summary>
    MemberAdministrator = 3,

    /// <summary>
    ///     文字频道管理员
    /// </summary>
    TextChannelAdministrator = 4,

    /// <summary>
    ///     语音频道管理员
    /// </summary>
    VoiceChannelAdministrator = 5,

    /// <summary>
    ///     社区共建者
    /// </summary>
    CommunityBuilder = 6,

    /// <summary>
    ///     高级管理员
    /// </summary>
    SeniorAdministrator = 7,

    /// <summary>
    ///     游客
    /// </summary>
    Guest = 254,

    /// <summary>
    ///     默认的 <c>@全体成员</c> 全体成员角色
    /// </summary>
    Everyone = 255
}
