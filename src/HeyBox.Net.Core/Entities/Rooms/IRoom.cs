namespace HeyBox;

/// <summary>
///     表示一个通用的房间。
/// </summary>
public interface IRoom : IEntity<ulong>
{
    /// <summary>
    ///     获取此房间的名称。
    /// </summary>
    string? Name { get; }

    /// <summary>
    ///     获取此房间图标的 URL。
    /// </summary>
    string? Icon { get; }

    /// <summary>
    ///     获取此房间内指定的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IRoomChannel?> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此房间内指定具有文字聊天能力的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    /// <remarks>
    ///     语音频道也是一种文字频道，此方法本意用于获取具有文字聊天能力的频道。如果通过此方法传入的 ID 对应的频道是语音频道，那么也会返回对应的语音频道实体。
    ///     如需获取频道的实际类型，请参考 <see cref="HeyBox.ChannelExtensions.GetChannelType(HeyBox.IChannel)"/>。
    /// </remarks>
    Task<ITextChannel?> GetTextChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    #region Roles

    /// <summary>
    ///     获取此房间的所有角色。
    /// </summary>
    IReadOnlyCollection<IRole> Roles { get; }

    /// <summary>
    ///     获取此房间中的 <c>@全体成员</c> 全体成员角色。
    /// </summary>
    IRole EveryoneRole { get; }

    /// <summary>
    ///     获取此房间内的角色。
    /// </summary>
    /// <param name="id"> 要获取的角色的 ID。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的角色；如果未找到，则返回 <c>null</c>。 </returns>
    IRole? GetRole(ulong id);

    /// <summary>
    ///     在此房间内创建一个新角色。
    /// </summary>
    /// <remarks>
    ///     此方法使用指定的属性创建新角色。要查看可用的属性，请参考 <see cref="HeyBox.RoleProperties"/>。
    /// </remarks>
    /// <param name="func"> 一个用于填充新角色属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的角色。 </returns>
    Task<IRole> CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options = null);

    #endregion

    #region Emotes

    /// <summary>
    ///     获取此服务器的所有自定义表情。
    /// </summary>
    IReadOnlyCollection<RoomEmote> Emotes { get; }

    /// <summary>
    ///     获取此服务器的所有自定义小表情。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此服务器的所有自定义小表情。 </returns>
    Task<IReadOnlyCollection<RoomEmote>> GetEmotesAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此服务器的指定自定义小表情。
    /// </summary>
    /// <param name="id"> 要获取的自定义表情的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的自定义小表情；如果未找到，则返回 <c>null</c>。 </returns>
    Task<RoomEmote?> GetEmoteAsync(ulong id, RequestOptions? options = null);

    /// <summary>
    ///     修改此服务器内的现有自定义小表情。
    /// </summary>
    /// <param name="emote"> 要修改的自定义小表情。 </param>
    /// <param name="func"> 一个用于设置自定义表情属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。任务的结果包含修改后的自定义表情。 </returns>
    Task ModifyEmoteAsync(RoomEmote emote, Action<EmoteProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     删除此服务器内的现有自定义小表情。
    /// </summary>
    /// <param name="emote"> 要删除的自定义小表情。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步删除操作的任务。 </returns>
    Task DeleteEmoteAsync(RoomEmote emote, RequestOptions? options = null);

    #endregion

    #region Stickers

    /// <summary>
    ///     获取此服务器的所有自定义大表情。
    /// </summary>
    IReadOnlyCollection<RoomSticker> Stickers { get; }

    /// <summary>
    ///     获取此服务器的所有自定义大表情。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此服务器的所有自定义大表情。 </returns>
    Task<IReadOnlyCollection<RoomSticker>> GetStickersAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此服务器的指定自定义大表情。
    /// </summary>
    /// <param name="id"> 要获取的自定义表情的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的自定义大表情；如果未找到，则返回 <c>null</c>。 </returns>
    Task<RoomSticker?> GetStickerAsync(ulong id, RequestOptions? options = null);

    /// <summary>
    ///     修改此服务器内的现有自定义大表情。
    /// </summary>
    /// <param name="emote"> 要修改的自定义大表情。 </param>
    /// <param name="func"> 一个用于设置自定义表情属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。任务的结果包含修改后的自定义表情。 </returns>
    Task ModifyStickerAsync(RoomSticker emote, Action<EmoteProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     删除此服务器内的现有自定义大表情。
    /// </summary>
    /// <param name="emote"> 要删除的自定义大表情。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步删除操作的任务。 </returns>
    Task DeleteStickerAsync(RoomSticker emote, RequestOptions? options = null);

    #endregion
}
