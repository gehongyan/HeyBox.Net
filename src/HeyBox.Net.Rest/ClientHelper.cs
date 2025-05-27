using System.Collections.Immutable;
using HeyBox.API;

namespace HeyBox.Rest;

internal static class ClientHelper
{
    public static async Task<Room?> GetRoomAsync(HeyBoxRestClient client, ulong id, RequestOptions? options)
    {
        await foreach (IReadOnlyCollection<Room> models in client.ApiClient.GetJoinedRoomsAsync(options: options).ConfigureAwait(false))
        {
            if (models.Count == 0)
                break;
            if (models.FirstOrDefault(x => x.RoomId == id) is { } model)
                return model;
        }

        return null;
    }

    public static async Task<IReadOnlyCollection<RestRoom>> GetRoomsAsync(HeyBoxRestClient client, RequestOptions? options)
    {
        ImmutableArray<RestRoom>.Builder guilds = ImmutableArray.CreateBuilder<RestRoom>();
        IEnumerable<Room> models = await client.ApiClient.GetJoinedRoomsAsync(options: options).FlattenAsync().ConfigureAwait(false);
        foreach (Room model in models)
            guilds.Add(RestRoom.Create(client, model));
        return guilds.ToImmutable();
    }
}
