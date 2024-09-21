namespace HeyBox.Interactions;

internal static class CommandHierarchy
{
    public const char EscapeChar = '$';

    public static IList<string> GetModulePath(this ModuleInfo moduleInfo) => [];

    public static IList<string> GetCommandPath(this ICommandInfo commandInfo) =>
        [..commandInfo.Module.GetModulePath(), commandInfo.Name];

    public static IList<string> GetParameterPath(this IParameterInfo parameterInfo) =>
        [..parameterInfo.Command.GetCommandPath(), parameterInfo.Name];

    public static IList<string> GetChoicePath(this IParameterInfo parameterInfo, ParameterChoice choice) =>
        [..parameterInfo.GetParameterPath(), choice.Name];

    public static IList<string> GetTypePath(Type type) => [$"{EscapeChar}{type.FullName}"];
}
