using HeyBox.API.Rest;

namespace HeyBox.WebSocket;

internal static class SocketRoomHelper
{
    public static async Task UpdateAsync(SocketRoom room, HeyBoxSocketClient client, RequestOptions? options)
    {
        GetRoomRolesResponse roles = await client.ApiClient.GetRoomRolesAsync(room.Id, options);
        room.Update(client.State, roles);
        GetRoomMemesResponse memes = await client.ApiClient.GetRoomMemesAsync(room.Id, options);
        room.Update(client.State, memes);
    }
}
