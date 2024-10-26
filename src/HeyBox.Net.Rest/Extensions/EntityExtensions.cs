namespace HeyBox.Rest;

internal static class EntityExtensions
{
    #region Emotes

    public static RoomEmote ToEmoteEntity(this API.Meme model, IRoomUser creator) =>
        new(creator.Room, creator, model.Name, model.Path, model.Extension, model.CreateTime);

    public static RoomSticker ToStickerEntity(this API.Meme model, IRoomUser creator) =>
        new(creator.Room, creator, model.Name, model.Path, model.Extension, model.CreateTime);

    #endregion
}
