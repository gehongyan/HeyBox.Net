using HeyBox.API.Rest;
using HeyBox.Rest;

namespace HeyBox.WebSocket;

internal static class SocketRoomHelper
{
    public static async Task UpdateAsync(SocketRoom room, HeyBoxSocketClient client, RequestOptions? options)
    {
        if (await ClientHelper.GetRoomAsync(client.Rest, room.Id, options) is { } model)
            room.Update(client.State, model);
        GetRoomRolesResponse roles = await client.ApiClient.GetRoomRolesAsync(room.Id, options);
        room.Update(client.State, roles);
        GetRoomMemesResponse memes = await client.ApiClient.GetRoomMemesAsync(room.Id, options);
        room.Update(client.State, memes);
    }
}
