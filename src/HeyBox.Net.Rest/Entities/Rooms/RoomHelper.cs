namespace HeyBox.Rest;

internal class RoomHelper
{
    public static Task<RestTextChannel> GetTextChannelAsync(IRoom room, BaseHeyBoxClient client,
        ulong id, RequestOptions? options) =>
        Task.FromResult(new RestTextChannel(client, room, id));
}
