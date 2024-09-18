namespace HeyBox;

/// <summary>
///     表示一个可以被提及的实体对象。
/// </summary>
public interface IMentionable
{
    /// <summary>
    ///     获取一个用于提及此对象的格式化字符串。
    /// </summary>
    string Mention { get; }
}