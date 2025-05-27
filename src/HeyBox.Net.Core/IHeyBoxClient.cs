namespace HeyBox;

/// <summary>
///     表示一个通用的黑盒语音 Bot 客户端。
/// </summary>
public interface IHeyBoxClient : IDisposable
{
    #region General

    /// <summary>
    ///     获取当前连接的状态。
    /// </summary>
    ConnectionState ConnectionState { get; }

    /// <summary>
    ///     获取已登录用户的令牌类型。
    /// </summary>
    TokenType TokenType { get; }

    /// <summary>
    ///     启动客户端与黑盒语音之间的连接。
    /// </summary>
    /// <remarks>
    ///     当前方法会初始化客户端与黑盒语音之间的连接。 <br />
    ///     <note type="important">
    ///         此方法会在调用后立即返回，因为它会在另一个线程上初始化连接。
    ///     </note>
    /// </remarks>
    /// <returns> 一个表示异步启动操作的任务。 </returns>
    Task StartAsync();

    /// <summary>
    ///     停止客户端与黑盒语音之间的连接。
    /// </summary>
    /// <returns> 一个表示异步停止操作的任务。 </returns>
    Task StopAsync();

    /// <summary>
    ///     登录到黑盒语音 API。
    /// </summary>
    /// <param name="tokenType"> 要使用的令牌类型。 </param>
    /// <param name="token"> 要使用的令牌。 </param>
    /// <param name="validateToken"> 是否验证令牌。 </param>
    /// <returns> 一个表示异步登录操作的任务。 </returns>
    /// <remarks>
    ///     验证令牌的操作是通过 <see cref="HeyBox.TokenUtils.ValidateToken(HeyBox.TokenType,System.String)"/> 方法完成的。 <br />
    ///     此方法用于向当前客户端设置后续 API 请求的身份验证信息，获取并设置当前所登录用户的信息。
    /// </remarks>
    Task LoginAsync(TokenType tokenType, string token, bool validateToken = true);

    /// <summary>
    ///     从黑盒语音 API 退出登录。
    /// </summary>
    /// <returns> 一个表示异步退出登录操作的任务。 </returns>
    /// <remarks>
    ///     此方法用于清除当前客户端的身份验证信息及所缓存的当前所登录的用户信息。
    /// </remarks>
    Task LogoutAsync();

    #endregion

    #region Room

    /// <summary>
    ///     获取一个房间。
    /// </summary>
    /// <param name="id"> 房间的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的房间；若指定 ID 的房间不存在，则为 <c>null</c>。 </returns>
    Task<IRoom?> GetRoomAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取当前用户所在的所有房间。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是当前用户所在的所有房间。 </returns>
    Task<IReadOnlyCollection<IRoom>> GetRoomsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    #endregion

    #region Users

    /// <summary>
    ///     获取一个用户。
    /// </summary>
    /// <param name="id"> 用户的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的用户；若指定 ID 的用户不存在，则为 <c>null</c>。 </returns>
    Task<IUser?> GetUserAsync(uint id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    #endregion

    #region Channels

    /// <summary>
    ///     获取一个频道。
    /// </summary>
    /// <param name="id"> 频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的频道；若指定 ID 的频道不存在，则为 <c>null</c>。 </returns>
    Task<IChannel?> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取一个私聊频道。
    /// </summary>
    /// <param name="userId"> 私聊频道中另一位用户的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定聊天代码的私聊频道；若指定聊天代码的私聊频道不存在，则为 <c>null</c>。 </returns>
    Task<IDMChannel?> GetDMChannelAsync(uint userId, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取当前会话中已创建的所有私聊频道。
    /// </summary>
    /// <remarks>
    ///     <note type="warning">
    ///         此方法不会返回当前会话之外已创建的私聊频道。如果客户端刚刚启动，这可能会返回一个空集合。
    ///     </note>
    /// </remarks>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是当前会话中已创建的所有私聊频道。 </returns>
    Task<IReadOnlyCollection<IDMChannel>> GetDMChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    #endregion
}
