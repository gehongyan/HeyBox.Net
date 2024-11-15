using System.Diagnostics.CodeAnalysis;

namespace HeyBox;

/// <summary>
///     用来构建 <see cref="CountdownModule"/> 模块的构建器。
/// </summary>
public class CountdownModuleBuilder : IModuleBuilder, IEquatable<CountdownModuleBuilder>, IEquatable<IModuleBuilder>
{
    /// <summary>
    ///     获取倒计时的最大时间间隔。
    /// </summary>
    public static readonly TimeSpan MaxCountdownTimeSpan = TimeSpan.FromDays(99);

    /// <inheritdoc />
    public ModuleType Type => ModuleType.Countdown;

    /// <summary>
    ///     初始化一个 <see cref="CountdownModuleBuilder"/> 类的新实例。
    /// </summary>
    public CountdownModuleBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="CountdownModuleBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="mode"> 倒计时的显示模式。 </param>
    /// <param name="endTime"> 倒计时结束的时间。 </param>
    public CountdownModuleBuilder(CountdownMode mode, DateTimeOffset endTime)
    {
        Mode = mode;
        EndTime = endTime;
    }

    /// <summary>
    ///     获取或设置倒计时结束的时间。
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    ///     获取或设置倒计时的显示模式。
    /// </summary>
    public CountdownMode Mode { get; set; }

    /// <summary>
    ///     设置倒计时的显示模式。
    /// </summary>
    /// <param name="mode"> 倒计时的显示模式。 </param>
    /// <returns> 当前构建器。 </returns>
    public CountdownModuleBuilder WithMode(CountdownMode mode)
    {
        Mode = mode;
        return this;
    }

    /// <summary>
    ///     设置倒计时结束的时间。
    /// </summary>
    /// <param name="endTime"> 倒计时结束的时间。 </param>
    /// <returns> 当前构建器。 </returns>
    public CountdownModuleBuilder WithEndTime(DateTimeOffset endTime)
    {
        EndTime = endTime;
        return this;
    }

    /// <summary>
    ///     设置倒计时结束的时间。
    /// </summary>
    /// <param name="interval"> 倒计时结束的时间与当前时间的时间间隔。 </param>
    /// <returns> 当前构建器。 </returns>
    public CountdownModuleBuilder WithEndTime(TimeSpan interval)
    {
        EndTime = DateTimeOffset.Now + interval;
        return this;
    }

    /// <summary>
    ///     构建当前构建器为一个 <see cref="CountdownModule"/> 对象。
    /// </summary>
    /// <returns> 由当前构建器表示的属性构建的 <see cref="CountdownModule"/> 对象。 </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <see cref="EndTime"/> 早于当前时间。
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <see cref="EndTime"/> 与当前时间的时间间隔超过 <see cref="MaxCountdownTimeSpan"/>。
    /// </exception>
    public CountdownModule Build()
    {
        if (EndTime < DateTimeOffset.Now)
            throw new ArgumentOutOfRangeException(
                nameof(EndTime),
                $"{nameof(EndTime)} must be equal or later than current timestamp.");

        if (EndTime - DateTimeOffset.Now > MaxCountdownTimeSpan)
            throw new ArgumentOutOfRangeException(
                nameof(EndTime),
                $"{nameof(EndTime)} must be within {MaxCountdownTimeSpan} from current timestamp.");

        return new CountdownModule(Mode, EndTime);
    }

    /// <inheritdoc />
    IModule IModuleBuilder.Build() => Build();

    /// <summary>
    ///     判定两个 <see cref="CountdownModuleBuilder"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="CountdownModuleBuilder"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(CountdownModuleBuilder? left, CountdownModuleBuilder? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>
    ///     判定两个 <see cref="CountdownModuleBuilder"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="CountdownModuleBuilder"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(CountdownModuleBuilder? left, CountdownModuleBuilder? right) =>
        !(left == right);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CountdownModuleBuilder builder && Equals(builder);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] CountdownModuleBuilder? countdownModuleBuilder)
    {
        if (countdownModuleBuilder is null) return false;

        return Type == countdownModuleBuilder.Type
            && EndTime == countdownModuleBuilder.EndTime
            && Mode == countdownModuleBuilder.Mode;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    bool IEquatable<IModuleBuilder>.Equals([NotNullWhen(true)] IModuleBuilder? moduleBuilder) =>
        Equals(moduleBuilder as CountdownModuleBuilder);
}
