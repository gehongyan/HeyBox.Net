namespace HeyBox.Rest;

internal static class EntityExtensions
{
    #region Emotes

    public static RoomEmote ToEmoteEntity(this API.Meme model, IRoomUser creator) =>
        new(model.Name, model.Path, creator.Room, creator, model.Extension, model.CreateTime);

    public static RoomSticker ToStickerEntity(this API.Meme model, IRoomUser creator) =>
        new(model.Name, model.Path, creator.Room, creator, model.Extension, model.CreateTime);

    #endregion
}
