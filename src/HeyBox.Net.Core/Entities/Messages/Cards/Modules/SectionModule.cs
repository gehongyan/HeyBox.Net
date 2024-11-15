using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     文本模块，可用于 <see cref="ICard"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SectionModule : IModule, IEquatable<SectionModule>, IEquatable<IModule>
{
    /// <inheritdoc />
    public ModuleType Type => ModuleType.Section;

    internal SectionModule(ImmutableArray<INode> paragraph)
    {
        Paragraph = paragraph;
    }

    /// <summary>
    ///     获取文本模块的节点。
    /// </summary>
    public ImmutableArray<INode> Paragraph { get; }

    private string DebuggerDisplay => $"{Type} ({Paragraph.Length} Node{(Paragraph.Length == 1 ? string.Empty : "s")})";

    /// <summary>
    ///     判定两个 <see cref="SectionModule"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="SectionModule"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(SectionModule left, SectionModule right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="SectionModule"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="SectionModule"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(SectionModule left, SectionModule right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is SectionModule sectionModule && Equals(sectionModule);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] SectionModule? sectionModule) =>
        GetHashCode() == sectionModule?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int)2166136261;
            hash = (hash * 16777619) ^ Type.GetHashCode();
            foreach (INode imageNode in Paragraph)
                hash = (hash * 16777619) ^ imageNode.GetHashCode();
            return hash;
        }
    }

    bool IEquatable<IModule>.Equals([NotNullWhen(true)] IModule? module) =>
        Equals(module as SectionModule);
}
