namespace HeyBox;

/// <summary>
///     表示一个通用的用户。
/// </summary>
public interface IUser : IEntity<uint>, IMentionable
{
    /// <summary>
    ///     获取此用户的用户名。
    /// </summary>
    string? Username { get; }

    /// <summary>
    ///     获取此用户是否为 Bot。
    /// </summary>
    bool? IsBot { get; }

    /// <summary>
    ///     获取此用户的头像图像的 URL。
    /// </summary>
    string? Avatar { get; }

    /// <summary>
    ///     获取用户头像装饰类型。
    /// </summary>
    string? AvatarDecorationType { get; }

    /// <summary>
    ///     获取此用户的头像装饰图像的 URL。
    /// </summary>
    string? AvatarDecorationUrl { get; }

    /// <summary>
    ///     获取此用户的等级。
    /// </summary>
    int? Level { get; }
}
