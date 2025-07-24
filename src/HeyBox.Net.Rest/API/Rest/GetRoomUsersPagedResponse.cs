using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomUsersPagedResponse : IPageInfoProvider
{
    [JsonPropertyName("user_info")]
    public required ExtendedRoomUser[] UserInfo { get; set; }

    [JsonPropertyName("user_count")]
    public required int UserCount { get; set; }

    /// <inheritdoc />
    int IPageInfoProvider.TotalCount => UserCount;
}
