namespace HeyBox.Rest;

internal static class ClientHelper
{
    public static Task<RestRoom> GetRoomAsync(BaseHeyBoxClient client, ulong id, RequestOptions? options) =>
        Task.FromResult(new RestRoom(client, id));
}
