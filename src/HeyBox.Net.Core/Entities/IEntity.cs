namespace HeyBox;

/// <summary>
///     表示一个通用的具有唯一标识符的实体。
/// </summary>
/// <typeparam name="TId"> 唯一标识符的类型。 </typeparam>
public interface IEntity<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    ///     获取此实体的唯一标识符。
    /// </summary>
    TId Id { get; }

    /// <summary>
    ///     获取此实体是否填充了真实数据。
    /// </summary>
    /// <remarks>
    ///     由于黑盒语音的 Bot 能力仍处于初步阶段，HeyBox.Net 无法从网关或 API 获取所有必要数据。当此实体的数据来源于网关或
    ///     API 时，此属性值为 <see langword="true"/>。当 HeyBox.Net 需要创建实体但无法从网关或 API 获取数据时，HeyBox.Net
    ///     会创建一个仅包含 ID 的实体，此时，此属性值为 <see langword="false"/>。
    /// </remarks>
    bool IsPopulated { get; }
}
