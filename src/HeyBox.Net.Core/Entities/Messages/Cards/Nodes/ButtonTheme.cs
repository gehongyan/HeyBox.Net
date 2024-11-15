namespace HeyBox;

/// <summary>
///     表示 <see cref="ButtonNode"/> 的主题。
/// </summary>
public enum ButtonTheme
{
    /// <summary>
    ///     表观表现为默认样式。
    /// </summary>
    /// <remarks>
    ///     按钮的背景色大体为浅灰色，浅色模式下为 <c>#DDE1E8</c>（http://www.color-hex.com/color/dde1e8），深色模式下为
    ///     <c>#4F545C</c>（http://www.color-hex.com/color/4f545c）。
    /// </remarks>
    Default,

    /// <summary>
    ///     外观表现为主要操作。
    /// </summary>
    /// <remarks>
    ///     按钮的背景色大体为蓝色 <c>#2B74D9</c>（http://www.color-hex.com/color/2b74d9）。
    /// </remarks>
    Primary,

    /// <summary>
    ///     外观表现为成功。
    /// </summary>
    /// <remarks>
    ///     按钮的背景色大体为绿色，浅色模式下为 <c>#24A54D</c>（http://www.color-hex.com/color/24a54d），深色模式下为
    ///     <c>#2D7D46</c>（http://www.color-hex.com/color/2d7d46）。
    /// </remarks>
    Success,

    /// <summary>
    ///     外观表现为危险。
    /// </summary>
    /// <remarks>
    ///     按钮的背景色大体为鲜红色，浅色模式下为 <c>#E53947</c>（http://www.color-hex.com/color/e53947），深色模式下为
    ///     <c>#D83C3F</c>（http://www.color-hex.com/color/d83c3f）。
    /// </remarks>
    Danger,
}