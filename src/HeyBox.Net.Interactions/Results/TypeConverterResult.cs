using System.Diagnostics.CodeAnalysis;

namespace HeyBox.Interactions;

/// <summary> 表示 <see cref="TypeConverter.ReadAsync(IInteractionContext, ISlashCommandInteractionDataOption, IServiceProvider)"/> 的结果类型。 </summary>
public struct TypeConverterResult : IResult
{
    /// <summary> 如果操作成功，获取转换的结果。 </summary>
    public object? Value { get; }

    /// <inheritdoc/>
    public InteractionCommandError? Error { get; }

    /// <inheritdoc/>
    public string? ErrorReason { get; }

    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess => !Error.HasValue;

    private TypeConverterResult(object? value, InteractionCommandError? error, string? reason)
    {
        Value = value;
        Error = error;
        ErrorReason = reason;
    }

    /// <summary> 返回一个无错误的 <see cref="TypeConverterResult" />。 </summary>
    public static TypeConverterResult FromSuccess(object? value) =>
        new TypeConverterResult(value, null, null);

    /// <summary> 返回一个带有 <see cref="InteractionCommandError.Exception" /> 和 <see cref="Exception.Message"/> 的 <see cref="TypeConverterResult" />。 </summary>
    /// <param name="exception"> 导致类型转换失败的异常。 </param>
    public static TypeConverterResult FromError(Exception exception) =>
        new TypeConverterResult(null, InteractionCommandError.Exception, exception.Message);

    /// <summary> 返回一个带有指定错误和原因的 <see cref="PreconditionResult" />。 </summary>
    /// <param name="error"> 错误类型。 </param>
    /// <param name="reason"> 失败原因。 </param>
    public static TypeConverterResult FromError(InteractionCommandError error, string reason) =>
        new TypeConverterResult(null, error, reason);

    /// <summary> 返回一个带有指定 <paramref name="result"/> 类型的 <see cref="PreconditionResult" />。 </summary>
    /// <param name="result"> 失败的结果。 </param>
    public static TypeConverterResult FromError(IResult result) =>
        new TypeConverterResult(null, result.Error, result.ErrorReason);

    /// <inheritdoc />
    public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
}
