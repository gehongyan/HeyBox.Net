using System.Collections.Immutable;

namespace HeyBox.Interactions;

/// <summary> 表示分组先决条件的结果类型。 </summary>
public class PreconditionGroupResult : PreconditionResult
{
    /// <summary> 获取此分组下所有先决条件的结果。 </summary>
    public IReadOnlyCollection<PreconditionResult>? Results { get; }

    private PreconditionGroupResult(InteractionCommandError? error, string? reason, IEnumerable<PreconditionResult>? results)
        : base(error, reason)
    {
        Results = results?.ToImmutableArray();
    }

    /// <summary> 获取一个无错误的 <see cref="PreconditionGroupResult" />。 </summary>
    public static new PreconditionGroupResult FromSuccess() => new(null, null, null);

    /// <summary> 获取一个 <see cref="PreconditionGroupResult" />，其 <see cref="InteractionCommandError.Exception" /> 及 <see cref="Exception.Message"/>。 </summary>
    /// <param name="exception"> 导致先决条件检查失败的异常。 </param>
    public static new PreconditionGroupResult FromError(Exception exception) =>
        new(InteractionCommandError.Exception, exception.Message, null);

    /// <summary> 获取一个 <see cref="PreconditionGroupResult" />，其类型为指定 <paramref name="result"/>。 </summary>
    /// <param name="result"> 失败的结果。 </param>
    public static new PreconditionGroupResult FromError(IResult result) =>
        new(result.Error, result.ErrorReason, null);

    /// <summary> 获取一个 <see cref="PreconditionGroupResult" />，其 <see cref="InteractionCommandError.UnmetPrecondition" /> 及指定原因。 </summary>
    /// <param name="reason"> 失败原因。 </param>
    /// <param name="results"> 此分组下所有先决条件的结果。 </param>
    public static PreconditionGroupResult FromError(string reason, IEnumerable<PreconditionResult> results) =>
        new(InteractionCommandError.UnmetPrecondition, reason, results);
}
