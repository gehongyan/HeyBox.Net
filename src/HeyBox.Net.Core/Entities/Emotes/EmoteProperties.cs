namespace HeyBox;

/// <summary>
///     提供用于修改 <see cref="HeyBox.RoomEmote"/> 与 <see cref="HeyBox.RoomSticker"/> 的属性的类。
/// </summary>
/// <seealso cref="IRoom.ModifyEmoteAsync"/>
/// <seealso cref="IRoom.ModifyStickerAsync"/>
public class EmoteProperties
{
    /// <summary>
    ///     获取或设置要设置到此表情符号的名称。
    /// </summary>
    public string? Name { get; set; }
}
