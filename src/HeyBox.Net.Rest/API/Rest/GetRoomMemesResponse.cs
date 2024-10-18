using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class GetRoomMemesResponse
{
    [JsonPropertyName("emoji")]
    public required RoomMeme[] Emojis { get; init; }

    [JsonPropertyName("sticker")]
    public required RoomMeme[] Stickers { get; init; }
}

internal class RoomMeme
{
    [JsonPropertyName("user_info")]
    public required RoomUser UserInfo { get; init; }

    [JsonPropertyName("meme_info")]
    public required Meme MemeInfo { get; init; }
}
