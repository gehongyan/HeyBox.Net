using HeyBox.API.Rest;

namespace HeyBox.Rest;

internal static class UserHelper
{
    public static async Task<string?> ModifyNicknameAsync(IRoomUser user, BaseHeyBoxClient client,
        string nickname, RequestOptions? options)
    {
        ModifyRoomMemberNicknameParams args = new()
        {
            RoomId = user.RoomId,
            Nickname = nickname,
            ToUserId = user.Id
        };
        await client.ApiClient.ModifyGuildMemberNicknameAsync(args, options).ConfigureAwait(false);
        return nickname;
    }

    public static async Task AddRolesAsync(RestRoomUser user, BaseHeyBoxClient client,
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

    public static async Task RemoveRolesAsync(RestRoomUser user, BaseHeyBoxClient client,
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

    public static RestDMChannel CreateDMChannel(RestUser user, BaseHeyBoxClient client) => new(client, user);

    public static RestDMChannel CreateDMChannel(uint userId, BaseHeyBoxClient client) =>
        CreateDMChannel(RestUser.Create(client, userId), client);
}
