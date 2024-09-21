namespace HeyBox;

/// <summary>
///     提供用于各种频道实体的扩展方法。
/// </summary>
public static class ChannelExtensions
{
    /// <summary>
    ///     尝试基于频道所实现的接口类型获取频道的实际类型。
    /// </summary>
    /// <param name="channel"> 要获取其类型的频道。 </param>
    /// <returns> 如果此频道的实际类型已知，则返回其类型；否则，返回 <c>null</c>。 </returns>
    public static ChannelType? GetChannelType(this IChannel channel) =>
        channel switch
        {
            ITextChannel => ChannelType.Text,
            _ => null
        };
}