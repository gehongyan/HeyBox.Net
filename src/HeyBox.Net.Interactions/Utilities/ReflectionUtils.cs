using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HeyBox.Interactions;

internal static class ReflectionUtils
{
    public static readonly TypeInfo ObjectTypeInfo = typeof(object).GetTypeInfo();
}

internal static class ReflectionUtils<T>
{
    internal static T CreateObject(TypeInfo typeInfo, InteractionService commandService, IServiceProvider? services = null) =>
        CreateBuilder(typeInfo, commandService)(services);

    internal static Func<IServiceProvider?, T> CreateBuilder(TypeInfo typeInfo, InteractionService commandService)
    {
        ConstructorInfo constructor = GetConstructor(typeInfo);
        ParameterInfo[] parameters = constructor.GetParameters();
        PropertyInfo[] properties = GetProperties(typeInfo);

        return (services) =>
        {
            object[] args = parameters
                .Select(x => GetMember(commandService, services, x.ParameterType, typeInfo, x.GetCustomAttributes()))
                .ToArray();

            T obj = InvokeConstructor(constructor, args, typeInfo);
            foreach (PropertyInfo property in properties)
                property.SetValue(obj, GetMember(commandService, services, property.PropertyType, typeInfo, null));
            return obj;
        };
    }

    private static T InvokeConstructor(ConstructorInfo constructor, object[] args, TypeInfo ownerType)
    {
        try
        {
            return (T)constructor.Invoke(args);
        }
        catch (Exception ex)
        {
            throw new TargetInvocationException($"Failed to create \"{ownerType.FullName}\".", ex);
        }
    }
    private static ConstructorInfo GetConstructor(TypeInfo ownerType)
    {
        ConstructorInfo[] constructors = ownerType.DeclaredConstructors.Where(x => !x.IsStatic).ToArray();
        if (constructors.Length == 0)
            throw new MissingMethodException($"No constructor found for \"{ownerType.FullName}\".");
        else if (constructors.Length > 1)
            throw new InvalidOperationException($"Multiple constructors found for \"{ownerType.FullName}\".");
        return constructors[0];
    }
    private static PropertyInfo[] GetProperties(TypeInfo ownerType)
    {
        TypeInfo? typeInfo = ownerType;
        List<PropertyInfo> result = [];
        while (typeInfo != ReflectionUtils.ObjectTypeInfo && typeInfo != null)
        {
            foreach (PropertyInfo prop in typeInfo.DeclaredProperties)
            {
                if (prop.SetMethod is { IsStatic: false, IsPublic: true })
                    result.Add(prop);
            }
            typeInfo = typeInfo.BaseType?.GetTypeInfo();
        }
        return result.ToArray();
    }
    private static object GetMember(InteractionService commandService, IServiceProvider? services, Type memberType, TypeInfo ownerType, IEnumerable<object>? attributes)
    {
        if (memberType == typeof(InteractionService)) return commandService;
        if (services is not null && (memberType == typeof(IServiceProvider) || memberType == services.GetType())) return services;
        object? service = attributes?.FirstOrDefault(x => x.GetType() == typeof(FromKeyedServicesAttribute)) is { } keyedAttribute
            ? services?.GetKeyedServices(memberType, ((FromKeyedServicesAttribute)keyedAttribute).Key).First()
            : services?.GetService(memberType);
        if (service != null) return service;
        throw new InvalidOperationException($"Failed to create \"{ownerType.FullName}\", dependency \"{memberType.Name}\" was not found.");
    }

    internal static Func<T, object?[], Task> CreateMethodInvoker(MethodInfo methodInfo)
    {
        ParameterInfo[] parameters = methodInfo.GetParameters();
        Expression[] paramsExp = new Expression[parameters.Length];

        ParameterExpression instanceExp = Expression.Parameter(typeof(T), "instance");
        ParameterExpression argsExp = Expression.Parameter(typeof(object?[]), "args");

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];

            ConstantExpression indexExp = Expression.Constant(i);
            BinaryExpression accessExp = Expression.ArrayIndex(argsExp, indexExp);
            paramsExp[i] = Expression.Convert(accessExp, parameter.ParameterType);
        }

        ArgumentNullException.ThrowIfNull(methodInfo.ReflectedType, nameof(methodInfo.ReflectedType));
        MethodCallExpression callExp = Expression.Call(Expression.Convert(instanceExp, methodInfo.ReflectedType), methodInfo, paramsExp);
        UnaryExpression finalExp = Expression.Convert(callExp, typeof(Task));
        Func<T, object?[], Task> lambda = Expression.Lambda<Func<T, object?[], Task>>(finalExp, instanceExp, argsExp).Compile();

        return lambda;
    }

    /// <summary>
    /// Create a type initializer using compiled lambda expressions
    /// </summary>
    internal static Func<IServiceProvider?, T> CreateLambdaBuilder(TypeInfo typeInfo, InteractionService commandService)
    {
        ConstructorInfo constructor = GetConstructor(typeInfo);
        ParameterInfo[] parameters = constructor.GetParameters();
        PropertyInfo[] properties = GetProperties(typeInfo);

        Func<object[], object[], T> lambda = CreateLambdaMemberInit(typeInfo, constructor);

        return (services) =>
        {
            object[] args = parameters
                .Select(x => GetMember(commandService, services, x.ParameterType, typeInfo, x.GetCustomAttributes()))
                .ToArray();
            object[] props = properties
                .Select(x => GetMember(commandService, services, x.PropertyType, typeInfo, null))
                .ToArray();
            T instance = lambda(args, props);
            return instance;
        };
    }

    internal static Func<object?[], T> CreateLambdaConstructorInvoker(TypeInfo typeInfo)
    {
        ConstructorInfo constructor = GetConstructor(typeInfo);
        ParameterInfo[] parameters = constructor.GetParameters();

        ParameterExpression argsExp = Expression.Parameter(typeof(object[]), "args");

        Expression[] parameterExps = new Expression[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            ConstantExpression indexExp = Expression.Constant(i);
            BinaryExpression accessExp = Expression.ArrayIndex(argsExp, indexExp);
            parameterExps[i] = Expression.Convert(accessExp, parameters[i].ParameterType);
        }

        NewExpression newExp = Expression.New(constructor, parameterExps);

        return Expression.Lambda<Func<object?[], T>>(newExp, argsExp).Compile();
    }

    /// <summary>
    ///     Create a compiled lambda property setter.
    /// </summary>
    internal static Action<T, object> CreateLambdaPropertySetter(PropertyInfo propertyInfo)
    {
        ParameterExpression instanceParam = Expression.Parameter(typeof(T), "instance");
        ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");

        MemberExpression prop = Expression.Property(instanceParam, propertyInfo);
        BinaryExpression assign = Expression.Assign(prop, Expression.Convert(valueParam, propertyInfo.PropertyType));

        return Expression.Lambda<Action<T, object>>(assign, instanceParam, valueParam).Compile();
    }

    internal static Func<T, object> CreateLambdaPropertyGetter(PropertyInfo propertyInfo)
    {
        ParameterExpression instanceParam = Expression.Parameter(typeof(T), "instance");
        MemberExpression prop = Expression.Property(instanceParam, propertyInfo);
        return Expression.Lambda<Func<T, object>>(prop, instanceParam).Compile();
    }

    internal static Func<T, object> CreateLambdaPropertyGetter(Type type, PropertyInfo propertyInfo)
    {
        ParameterExpression instanceParam = Expression.Parameter(typeof(T), "instance");
        UnaryExpression instanceAccess = Expression.Convert(instanceParam, type);
        MemberExpression prop = Expression.Property(instanceAccess, propertyInfo);
        return Expression.Lambda<Func<T, object>>(prop, instanceParam).Compile();
    }

    internal static Func<object[], object[], T> CreateLambdaMemberInit(TypeInfo typeInfo, ConstructorInfo constructor, Predicate<PropertyInfo>? propertySelect = null)
    {
        propertySelect ??= x => true;

        ParameterInfo[] parameters = constructor.GetParameters();
        PropertyInfo[] properties = GetProperties(typeInfo).Where(x => propertySelect(x)).ToArray();

        ParameterExpression argsExp = Expression.Parameter(typeof(object[]), "args");
        ParameterExpression propsExp = Expression.Parameter(typeof(object[]), "props");

        Expression[] parameterExps = new Expression[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            ConstantExpression indexExp = Expression.Constant(i);
            BinaryExpression accessExp = Expression.ArrayIndex(argsExp, indexExp);
            parameterExps[i] = Expression.Convert(accessExp, parameters[i].ParameterType);
        }

        NewExpression newExp = Expression.New(constructor, parameterExps);

        MemberBinding[] memberExps = new MemberBinding[properties.Length];

        for (int i = 0; i < properties.Length; i++)
        {
            ConstantExpression indexEx = Expression.Constant(i);
            UnaryExpression accessExp = Expression.Convert(Expression.ArrayIndex(propsExp, indexEx), properties[i].PropertyType);
            memberExps[i] = Expression.Bind(properties[i], accessExp);
        }
        MemberInitExpression memberInit = Expression.MemberInit(newExp, memberExps);
        Func<object[], object[], T> lambda = Expression.Lambda<Func<object[], object[], T>>(memberInit, argsExp, propsExp).Compile();

        return (args, props) =>
        {
            T instance = lambda(args, props);

            return instance;
        };
    }
}
