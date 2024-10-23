using HeyBox.API;
using HeyBox.API.Rest;

namespace HeyBox.Rest;

internal static class RoleHelper
{
    public static async Task<Role> ModifyAsync(IRole role, BaseHeyBoxClient client,
        Action<RoleProperties> func, RequestOptions? options)
    {
        RoleProperties properties = new();
        func(properties);
        Preconditions.NotNullOrEmpty(properties.Name, nameof(properties.Name));
        ModifyRoomRoleParams args = new()
        {
            Id = role.Id,
            Name = properties.Name,
            Icon = properties.Icon,
            ColorList = properties.GradientColor,
            RoomId = role.Room.Id,
            Permissions = (properties.Permissions ?? RoomPermissions.None).RawValue,
            Type = role.Type,
            Color = properties.Color,
            Hoist = properties.Hoist ?? false,
            Nonce = string.Empty
        };
        return await client.ApiClient.ModifyRoomRoleAsync(args, options);
    }

    public static async Task DeleteAsync(IRole role, BaseHeyBoxClient client, RequestOptions? options)
    {
        DeleteRoomRoleParams args = new()
        {
            RoleId = role.Id,
            RoomId = role.Room.Id
        };
        await client.ApiClient.DeleteRoomRoleAsync(args, options).ConfigureAwait(false);
    }
}
