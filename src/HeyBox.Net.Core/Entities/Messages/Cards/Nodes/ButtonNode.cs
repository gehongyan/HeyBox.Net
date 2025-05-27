using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     按钮节点，可用于 <see cref="HeyBox.IModule"/> 中。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ButtonNode : INode, IEquatable<ButtonNode>, IEquatable<INode>
{
    /// <inheritdoc />
    public NodeType Type => NodeType.Button;

    internal ButtonNode(string text, ButtonEvent @event, string value, ButtonTheme theme, NodeWidth? width)
    {
        Theme = theme;
        Width = width;
        Value = value;
        Event = @event;
        Text = text;
    }

    /// <summary>
    ///     获取按钮节点的文本。
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     获取按钮节点的主题。
    /// </summary>
    public ButtonTheme Theme { get; }

    /// <summary>
    ///     获取按钮节点的值。
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     获取按钮节点的事件。
    /// </summary>
    public ButtonEvent Event { get; }

    /// <inheritdoc />
    public NodeWidth? Width { get; }

    private string DebuggerDisplay => $"{Type}: {Text} ({Event}, {Value}, {Theme})";

    /// <summary>
    ///     判定两个 <see cref="ButtonNode"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonNode"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ButtonNode? left, ButtonNode? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="ButtonNode"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="ButtonNode"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ButtonNode? left, ButtonNode? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ButtonNode buttonNode && Equals(buttonNode);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] ButtonNode? buttonNode) =>
        GetHashCode() == buttonNode?.GetHashCode();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (int) 2166136261;
            hash = (hash * 16777619) ^ HashCode.Combine(Type, Theme, Value, Event);
            hash = (hash * 16777619) ^ Text.GetHashCode();
            return hash;
        }
    }

    bool IEquatable<INode>.Equals([NotNullWhen(true)] INode? node) =>
        Equals(node as ButtonNode);
}
