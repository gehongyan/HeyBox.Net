namespace HeyBox.Interactions;

internal class CommandMap<T> where T : class, ICommandInfo
{
    private readonly char[] _separators;

    private readonly CommandMapNode<T> _root;
    private readonly InteractionService _commandService;

    public IReadOnlyCollection<char> Separators => _separators;

    public CommandMap(InteractionService commandService, char[]? separators = null)
    {
        _separators = separators ?? [];

        _commandService = commandService;
        _root = new CommandMapNode<T>(null, _commandService._wildCardExp);
    }

    public void AddCommand(T command, bool ignoreGroupNames = false)
    {
        if (ignoreGroupNames)
            AddCommandToRoot(command);
        else
            AddCommand(command);
    }

    public void AddCommandToRoot(T command)
    {
        string[] key = [command.Name];
        _root.AddCommand(key, 0, command);
    }

    public void AddCommand(IList<string> input, T command)
    {
        _root.AddCommand(input, 0, command);
    }

    public void RemoveCommand(T command)
    {
        var key = CommandHierarchy.GetCommandPath(command);

        _root.RemoveCommand(key, 0);
    }

    public SearchResult<T> GetCommand(string input)
    {
        if (_separators.Any())
            return GetCommand(input.Split(_separators));
        else
            return GetCommand([input]);
    }

    public SearchResult<T> GetCommand(IList<string> input) =>
        _root.GetCommand(input, 0);

    private void AddCommand(T command)
    {
        var key = CommandHierarchy.GetCommandPath(command);

        _root.AddCommand(key, 0, command);
    }
}
