using HeyBox.API.Rest;
using HeyBox.WebSocket;

namespace HeyBox.WebSocket;

internal static class SocketUserHelper
{
    public static async Task AddRolesAsync(SocketRoomUser user, BaseSocketClient client,
        IEnumerable<ulong> roleIds, RequestOptions? options)
    {
        IEnumerable<AddOrRemoveRoleParams> args = roleIds
            .Distinct()
            .Select(x => new AddOrRemoveRoleParams
            {
                RoomId = user.Room.Id,
                RoleId = x,
                UserId = user.Id
            });
        foreach (AddOrRemoveRoleParams arg in args)
        {
            await client.ApiClient.AddRoleAsync(arg, options).ConfigureAwait(false);
            user.AddRole(arg.RoleId);
        }
    }

    public static async Task RemoveRolesAsync(SocketRoomUser user, BaseSocketClient client,
        IEnumerable<ulong> roleIds, RequestOptions? options)
    {
        IEnumerable<AddOrRemoveRoleParams> args = roleIds
            .Distinct()
            .Select(x => new AddOrRemoveRoleParams
            {
                RoomId = user.Room.Id,
                RoleId = x,
                UserId = user.Id
            });
        foreach (AddOrRemoveRoleParams arg in args)
        {
            await client.ApiClient.RemoveRoleAsync(arg, options).ConfigureAwait(false);
            user.RemoveRole(arg.RoleId);
        }
    }

    public static SocketDMChannel CreateDMChannel(SocketUser user, HeyBoxSocketClient client) => new(client, user);

    public static SocketDMChannel CreateDMChannel(uint userId, HeyBoxSocketClient client)
    {
        SocketUser user = client.GetUser(userId) ?? new SocketGlobalUser(client, userId);
        return CreateDMChannel(user, client);
    }
}
