namespace HeyBox.Interactions;

/// <summary>
///     将参数注册为复杂参数。
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class ComplexParameterAttribute : Attribute
{
    /// <summary>
    ///     获取应优先使用的构造方法参数数组。
    /// </summary>
    public Type[]? PrioritizedCtorSignature { get; }

    /// <summary>
    ///     将斜线命令参数注册为复杂参数。
    /// </summary>
    public ComplexParameterAttribute() { }

    /// <summary>
    ///     将斜线命令参数注册为具有指定构造方法签名的复杂参数。
    /// </summary>
    /// <param name="types"> 首选构造方法参数的类型数组。 </param>
    public ComplexParameterAttribute(Type[] types)
    {
        PrioritizedCtorSignature = types;
    }
}
