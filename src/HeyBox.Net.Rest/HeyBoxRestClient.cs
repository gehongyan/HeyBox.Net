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
        if (disposing) ApiClient.Dispose();
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    internal override Task OnLoginAsync(TokenType tokenType, string token)
    {
        ApiClient.CurrentUserId = TokenUtils.TryParseBotTokenUserId(token, out ulong userId)
            ? userId
            : null;
        return Task.CompletedTask;
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
    /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的服务器；若指定 ID 的服务器不存在，则为 <c>null</c>。 </returns>
    public Task<RestRoom> GetRoomAsync(ulong id, RequestOptions? options = null) =>
        ClientHelper.GetRoomAsync(this, id, options);

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
