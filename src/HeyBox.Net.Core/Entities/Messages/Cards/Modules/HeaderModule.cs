using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     标题模块，可用于 <see cref="ICard"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class HeaderModule : IModule, IEquatable<HeaderModule>, IEquatable<IModule>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.Header;

    internal HeaderModule(ITextNode content)
    {
        Content = content;
    }

    /// <summary>
    ///     获取标题模块的文本。
    /// </summary>
    public ITextNode Content { get; }

    /// <inheritdoc />
    public override string? ToString() => Content?.ToString();

    private string DebuggerDisplay => $"{Type}: {Content}";

    /// <summary>
    ///     判定两个 <see cref="HeaderModule"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="HeaderModule"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(HeaderModule left, HeaderModule right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="HeaderModule"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="HeaderModule"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(HeaderModule left, HeaderModule right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is HeaderModule headerModule && Equals(headerModule);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] HeaderModule? headerModule) =>
        GetHashCode() == headerModule?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ Type.GetHashCode();
            hash = (hash * 16777619) ^ (Content?.GetHashCode() ?? 0);
            return hash;
        }
    }

    bool IEquatable<IModule>.Equals([NotNullWhen(true)] IModule? module) =>
        Equals(module as HeaderModule);
}
