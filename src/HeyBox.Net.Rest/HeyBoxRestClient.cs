using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Rest;

/// <summary>
///     表示一个基于 REST 的 HeyBox 客户端。
/// </summary>
public class HeyBoxRestClient : BaseHeyBoxClient, IHeyBoxClient
{
    #region HeyBoxRestClient

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    /// <inheritdoc cref="HeyBox.Rest.BaseHeyBoxClient.CurrentUser" />
    public new RestSelfUser? CurrentUser
    {
        get => base.CurrentUser as RestSelfUser;
        internal set => base.CurrentUser = value;
    }

    /// <summary>
    ///     使用默认配置初始化一个 <see cref="HeyBoxRestClient"/> 类的新实例。
    /// </summary>
    public HeyBoxRestClient()
        : this(new HeyBoxRestConfig())
    {
    }

    /// <summary>
    ///     使用指定的配置初始化一个 <see cref="HeyBoxRestClient"/> 类的新实例。
    /// </summary>
    /// <param name="config"> 用于初始化客户端的配置。 </param>
    public HeyBoxRestClient(HeyBoxRestConfig config)
        : base(config, CreateApiClient(config))
    {
    }

    internal HeyBoxRestClient(HeyBoxRestConfig config, API.HeyBoxRestApiClient api)
        : base(config, api)
    {
    }

    private static API.HeyBoxRestApiClient CreateApiClient(HeyBoxRestConfig config) =>
        new(config.RestClientProvider, HeyBoxConfig.UserAgent, config.DefaultRetryMode, SerializerOptions);

    internal override void Dispose(bool disposing)
    {
        if (disposing)
            ApiClient.Dispose();
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    internal override Task OnLoginAsync(TokenType tokenType, string token)
    {
        if (TokenUtils.TryParseBotTokenUserId(token, out uint userId))
        {
            ApiClient.CurrentUserId = userId;
            base.CurrentUser = new RestSelfUser(this, userId);
        }
        return Task.CompletedTask;
    }

    internal void CreateRestSelfUser(uint userId)
    {
        base.CurrentUser = new RestSelfUser(this, userId);
    }

    /// <inheritdoc />
    internal override Task OnLogoutAsync() => Task.CompletedTask;

    #endregion

    #region Rooms

    /// <summary>
    ///     获取一个房间。
    /// </summary>
    /// <param name="id"> 房间的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的房间；若指定 ID 的房间不存在，则为 <c>null</c>。 </returns>
    public Task<RestRoom> GetRoomAsync(ulong id, RequestOptions? options = null) =>
        ClientHelper.GetRoomAsync(this, id, options);

    #endregion

    #region Reactions

    /// <summary>
    ///     向指定的消息添加一个回应。
    /// </summary>
    /// <param name="messageId"> 要为其添加回应的消息的 ID。 </param>
    /// <param name="channelId"> 要为其添加回应的消息所在的频道的 ID。 </param>
    /// <param name="roomId"> 要为其添加回应的消息所在的房间的 ID。 </param>
    /// <param name="emote"> 要用于向指定消息添加回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示添加添加异步操作的任务。 </returns>
    public Task AddReactionAsync(ulong messageId, ulong channelId, ulong roomId, IEmote emote, RequestOptions? options = null) =>
        MessageHelper.AddReactionAsync(messageId, channelId, roomId, emote, this, options);

    /// <summary>
    ///     从指定的消息移除一个回应。
    /// </summary>
    /// <param name="messageId"> 要从中移除回应的消息的 ID。 </param>
    /// <param name="channelId"> 要从中移除回应的消息所在的频道的 ID。 </param>
    /// <param name="roomId"> 要从中移除回应的消息所在的房间的 ID。 </param>
    /// <param name="emote"> 要从指定消息移除的回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步移除操作的任务。 </returns>
    public Task RemoveReactionAsync(ulong messageId, ulong channelId, ulong roomId, IEmote emote, RequestOptions? options = null) =>
        MessageHelper.RemoveReactionAsync(messageId, channelId, roomId, emote, this, options);

    #endregion

    #region IHeyBoxClient

    /// <inheritdoc />
    async Task<IRoom?> IHeyBoxClient.GetRoomAsync(ulong id, CacheMode mode, RequestOptions? options)
    {
        if (mode == CacheMode.AllowDownload)
            return await GetRoomAsync(id, options).ConfigureAwait(false);
        return null;
    }

    #endregion

}
