namespace HeyBox.Interactions.Builders;

/// <summary>
///     表示用于创建 <see cref="CommandParameterInfo"/> 的参数生成器。
/// </summary>
public sealed class CommandParameterBuilder : ParameterBuilder<CommandParameterInfo, CommandParameterBuilder>
{
    /// <inheritdoc />
    protected override CommandParameterBuilder Instance => this;

    internal CommandParameterBuilder(ICommandBuilder command) : base(command) { }

    /// <summary>
    ///     初始化一个新的 <see cref="CommandParameterInfo"/>。
    /// </summary>
    /// <param name="command"> 此参数所属的父命令。 </param>
    /// <param name="name"> 此命令的名称。 </param>
    /// <param name="type"> 此参数的类型。 </param>
    public CommandParameterBuilder(ICommandBuilder command, string name, Type type)
        : base(command, name, type) { }

    internal override CommandParameterInfo Build(ICommandInfo command) => new(this, command);
}
