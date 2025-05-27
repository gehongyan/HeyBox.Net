using System.Globalization;

namespace HeyBox;

/// <summary>
/// 表示一个 CSS 颜色值，参见
/// <see href="https://developer.mozilla.org/docs/Web/CSS/color_value">&lt;color&gt; - CSS: Cascading Style Sheets | MDN</see>。
/// </summary>
public readonly struct CssColor : IEquatable<CssColor>
{
    /// <summary>
    ///     获取颜色的字符串值。
    /// </summary>
    public string Value { get; } = string.Empty;

    /// <summary>
    ///     获取一个表示默认颜色的 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <remarks>
    ///     黑盒语音客户端将使用等同于 <c>CssColor.FromVariable("--brank-fill")</c> 的值。
    /// </remarks>
    public static CssColor Default { get; } = new(string.Empty);

    /// <summary>
    ///     获取一个表示透明颜色的 <see cref="CssColor"/> 实例。
    /// </summary>
    public static CssColor Transparent { get; } = new("transparent");

    /// <summary>
    ///     使用指定的颜色字符串值初始化 <see cref="CssColor"/> 结构。
    /// </summary>
    /// <param name="value">颜色的字符串值。</param>
    private CssColor(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     通过颜色值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="value"> CSS 颜色值字符串。</param>
    /// <returns> 对应的 <see cref="CssColor"/> 实例。</returns>
    public static CssColor FromValue(string value) => new(value);

    /// <summary>
    ///     通过颜色名称创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="name">CSS 颜色名称。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="name"/> 为空或无效时抛出。</exception>
    public static CssColor FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Color name cannot be null or empty.", nameof(name));
        return new CssColor(name);
    }

    /// <summary>
    ///     通过十六进制颜色值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="hex">十六进制颜色值（如 <c>#FFF</c> 或 <c>#FFFFFF</c>）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="hex"/> 格式无效时抛出。</exception>
    public static CssColor FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex) ||
            hex.Length is not (4 or 7) ||
            !hex.StartsWith('#'))
            throw new ArgumentException("Invalid hex color format.", nameof(hex));
        return new CssColor(hex);
    }

    /// <summary>
    ///     通过 RGB 值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="r">红色分量（0-255）。</param>
    /// <param name="g">绿色分量（0-255）。</param>
    /// <param name="b">蓝色分量（0-255）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    public static CssColor FromRgb(byte r, byte g, byte b) => new($"rgb({r}, {g}, {b})");

    /// <summary>
    ///     通过 RGBA 值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="r">红色分量（0-255）。</param>
    /// <param name="g">绿色分量（0-255）。</param>
    /// <param name="b">蓝色分量（0-255）。</param>
    /// <param name="a">透明度（0-1）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="a"/> 不在 0 到 1 范围内时抛出。</exception>
    public static CssColor FromRgba(byte r, byte g, byte b, float a)
    {
        if (a is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(a), "Alpha must be between 0 and 1.");
        return new CssColor($"rgba({r}, {g}, {b}, {a.ToString(CultureInfo.InvariantCulture)})");
    }

    /// <summary>
    ///     通过 HSL 值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="h">色调（0-360）。</param>
    /// <param name="s">饱和度（0-100）。</param>
    /// <param name="l">亮度（0-100）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="h"/>、<paramref name="s"/> 或 <paramref name="l"/> 不在有效范围内时抛出。</exception>
    public static CssColor FromHsl(float h, float s, float l)
    {
        if (h is < 0 or >= 360)
            throw new ArgumentOutOfRangeException(nameof(h), "Hue must be between 0 and 360.");
        if (s is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(s), "Saturation must be between 0 and 100.");
        if (l is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(l), "Lightness must be between 0 and 100.");
        return new CssColor($"hsl({h.ToString(CultureInfo.InvariantCulture)}, {s.ToString(CultureInfo.InvariantCulture)}%, {l.ToString(CultureInfo.InvariantCulture)}%)");
    }

    /// <summary>
    ///     通过 HSLA 值创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="h">色调（0-360）。</param>
    /// <param name="s">饱和度（0-100）。</param>
    /// <param name="l">亮度（0-100）。</param>
    /// <param name="a">透明度（0-1）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="h"/>、<paramref name="s"/>、<paramref name="l"/> 或 <paramref name="a"/> 不在有效范围内时抛出。</exception>
    public static CssColor FromHsla(float h, float s, float l, float a)
    {
        if (h is < 0 or >= 360)
            throw new ArgumentOutOfRangeException(nameof(h), "Hue must be between 0 and 360.");
        if (s is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(s), "Saturation must be between 0 and 100.");
        if (l is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(l), "Lightness must be between 0 and 100.");
        if (a is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(a), "Alpha must be between 0 and 1.");
        return new CssColor($"hsla({h.ToString(CultureInfo.InvariantCulture)}, {s.ToString(CultureInfo.InvariantCulture)}%, {l.ToString(CultureInfo.InvariantCulture)}%, {a.ToString(CultureInfo.InvariantCulture)})");
    }

    /// <summary>
    ///     通过 CSS 变量名创建 <see cref="CssColor"/> 实例。
    /// </summary>
    /// <param name="variableName">CSS 变量名（不包含 <c>var()</c> 前缀，例如 <c>--brand-fill</c>）。</param>
    /// <returns>对应的 <see cref="CssColor"/> 实例。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="variableName"/> 为空或无效时抛出。</exception>
    public static CssColor FromVariable(string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentException("CSS variable name cannot be null or empty.", nameof(variableName));
        return new CssColor($"var(--{variableName.TrimStart('-')})");
    }

    /// <inheritdoc cref="HeyBox.CssColor.Value" />
    public override string ToString() => Value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CssColor other && Equals(other);

    /// <inheritdoc />
    public bool Equals(CssColor other) => Value == other.Value;

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// 确定两个 <see cref="CssColor"/> 实例是否相等。
    /// </summary>
    /// <param name="left">左侧的 <see cref="CssColor"/> 实例。</param>
    /// <param name="right">右侧的 <see cref="CssColor"/> 实例。</param>
    /// <returns>如果相等，则为 true；否则为 false。</returns>
    public static bool operator ==(CssColor left, CssColor right) => left.Equals(right);

    /// <summary>
    /// 确定两个 <see cref="CssColor"/> 实例是否不相等。
    /// </summary>
    /// <param name="left">左侧的 <see cref="CssColor"/> 实例。</param>
    /// <param name="right">右侧的 <see cref="CssColor"/> 实例。</param>
    /// <returns>如果不相等，则为 true；否则为 false。</returns>
    public static bool operator !=(CssColor left, CssColor right) => !(left == right);
}
