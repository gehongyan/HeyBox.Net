using HeyBox.API.Rest;

namespace HeyBox.Rest;

internal class RoomHelper
{
    public static async Task UpdateAsync(RestRoom room, BaseHeyBoxClient client, RequestOptions? options)
    {
        GetRoomRolesResponse roles = await client.ApiClient.GetRoomRolesAsync(room.Id, options);
        room.Update(roles);
    }

    public static Task<RestTextChannel> GetTextChannelAsync(IRoom room, BaseHeyBoxClient client,
        ulong id, RequestOptions? options) =>
        Task.FromResult(new RestTextChannel(client, room, id));
}
