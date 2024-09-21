namespace HeyBox.Interactions;

internal class EmptyServiceProvider : IServiceProvider
{
    public static EmptyServiceProvider Instance => new();

    public object? GetService(Type serviceType) => null;
}
