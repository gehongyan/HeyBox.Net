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

    public static async Task<API.Role> CreateRoleAsync(IRoom room, BaseHeyBoxClient client,
        Action<RoleProperties> func, RequestOptions? options)
    {
        RoleProperties properties = new();
        func(properties);
        Preconditions.NotNullOrEmpty(properties.Name, nameof(properties.Name));
        CreateRoomRoleParams args = new()
        {
            Name = properties.Name,
            Icon = properties.Icon,
            ColorList = properties.GradientColor,
            RoomId = room.Id,
            Permissions = (properties.Permissions ?? RoomPermissions.None).RawValue,
            Type = RoleType.Custom,
            Color = properties.Color,
            Hoist = properties.Hoist ?? false,
            Nonce = string.Empty
        };
        return await client.ApiClient.CreateRoomRoleAsync(args, options);
    }

    public static async Task ModifyMemeAsync(IRoom room, BaseHeyBoxClient client,
        IRoomEmote emote, Action<EmoteProperties> func, RequestOptions? options)
    {
        EmoteProperties properties = new()
        {
            Name = emote.Name
        };
        func(properties);
        Preconditions.NotNullOrEmpty(properties.Name, nameof(properties.Name));
        ModifyRoomMemeParams args = new()
        {
            RoomId = room.Id,
            Path = emote.Path,
            Name = properties.Name
        };
        await client.ApiClient.ModifyRoomMemeAsync(args, options);
    }

    public static async Task DeleteMemeAsync(IRoom room, BaseHeyBoxClient client,
        IRoomEmote emote, RequestOptions? options)
    {
        DeleteRoomMemeParams args = new()
        {
            RoomId = room.Id,
            Path = emote.Path
        };
        await client.ApiClient.DeleteRoomMemeAsync(args, options);
    }
}
