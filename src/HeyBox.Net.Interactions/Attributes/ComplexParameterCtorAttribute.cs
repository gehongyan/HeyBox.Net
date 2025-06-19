namespace HeyBox.Interactions;

/// <summary>
///     标记类型构造函数为首选的复杂命令构造函数。
/// </summary>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
public class ComplexParameterCtorAttribute : Attribute { }
