namespace HeyBox;

/// <summary>
///     表示一个通用的身份组。
/// </summary>
public interface IRole : IEntity<ulong>, IMentionable
{
    /// <summary>
    ///     获取拥有此角色的房间。
    /// </summary>
    IRoom Room { get; }

    /// <summary>
    ///     获取此角色的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此角色的图标。
    /// </summary>
    string Icon { get; }

    /// <summary>
    ///     获取此角色的类型。
    /// </summary>
    RoleType Type { get; }

    /// <summary>
    ///     获取此角色的颜色。
    /// </summary>
    /// <remarks>
    ///     如果此用户所拥有的最高角色的颜色类型为渐变色，则此属性返回的颜色是渐变色权益失效后的回退颜色。
    /// </remarks>
    Color Color { get; }

    /// <summary>
    ///     获取此角色的颜色类型。
    /// </summary>
    ColorType ColorType { get; }

    /// <summary>
    ///     获取此角色的渐变色。
    /// </summary>
    /// <remarks>
    ///     如果此角色的颜色类型 <see cref="HeyBox.IRole.ColorType"/> 不为
    ///     <see cref="HeyBox.ColorType.Gradient"/>，则此属性会返回 <see langword="null"/>。
    /// </remarks>
    GradientColor? GradientColor { get; }

    /// <summary>
    ///     获取此角色在房间角色列表中的位置。
    /// </summary>
    /// <remarks>
    ///     更小的数值表示更靠近列表顶部的位置。
    /// </remarks>
    int Position { get; }

    /// <summary>
    ///     获取拥有此角色的用户是否在用户列表中与普通在线成员分开显示。
    /// </summary>
    bool IsHoisted { get; }

    /// <summary>
    ///     获取是否允许任何人提及此角色。
    /// </summary>
    bool IsMentionable { get; }

    /// <summary>
    ///     获取此角色拥有的权限。
    /// </summary>
    RoomPermissions Permissions { get; }

    /// <summary>
    ///     获取此角色的创建时间。
    /// </summary>
    DateTimeOffset? CreatedAt { get; }

    /// <summary>
    ///     获取此角色的创建者 ID。
    /// </summary>
    ulong CreatorId { get; }
}
