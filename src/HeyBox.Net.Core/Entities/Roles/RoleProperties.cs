namespace HeyBox;

/// <summary>
///     提供用于创建角色的属性。
/// </summary>
public class RoleProperties
{
    /// <summary>
    ///     获取或设置要设置到此角色的名称。
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色的图标的 URL。
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色的颜色。
    /// </summary>
    public Color? Color { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色的渐变色。
    /// </summary>
    public GradientColor? GradientColor { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色的房间权限集。
    /// </summary>
    public RoomPermissions? Permissions { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色是否与其它角色分离显示。
    /// </summary>
    public bool? Hoist { get; set; }
}
