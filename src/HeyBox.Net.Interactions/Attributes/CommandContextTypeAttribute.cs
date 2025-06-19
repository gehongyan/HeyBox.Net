using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary>
///     指定此命令可在哪些上下文类型中执行。
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CommandContextTypeAttribute : Attribute
{
    /// <summary>
    ///     获取此命令可执行的上下文类型。
    /// </summary>
    public IReadOnlyCollection<InteractionContextType> ContextTypes { get; }

    /// <summary>
    ///     设置应用命令或模块的 <see cref="IApplicationCommandInfo.ContextTypes"/> 属性。
    /// </summary>
    /// <param name="contextTypes"> 为命令设置的上下文类型。 </param>
    public CommandContextTypeAttribute(params InteractionContextType[] contextTypes)
    {
        ContextTypes = contextTypes.Distinct().ToImmutableArray();

        if (ContextTypes.Count == 0)
            throw new ArgumentException("A command must have at least one supported context type.", nameof(contextTypes));
    }
}
