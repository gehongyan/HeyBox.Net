using HeyBox.Interactions;

namespace InteractionFramework.Modules;

public class ExampleModule : InteractionModuleBase<SocketInteractionContext>
{
    private InteractionHandler _handler;
    private readonly InteractionService _interactionService;

    // Constructor injection is also a valid way to access the dependencies
    public ExampleModule(InteractionHandler handler, InteractionService interactionService)
    {
        _handler = handler;
        _interactionService = interactionService;
    }

    // You can use a number of parameter types in you Slash Command handlers (string, int, double, bool, IUser, IChannel, IMentionable, IRole, Enums) by default. Optionally,
    // you can implement your own TypeConverters to support a wider range of parameter types. For more information, refer to the library documentation.
    // Optional method parameters(parameters with a default value) also will be displayed as optional on HeyBox.

    // [Summary] lets you customize the name and the description of a parameter
    [SlashCommand("echo", "Repeat the input")]
    public async Task Echo(string echo, [Summary(description: "mention the user")] bool mention = false)
        => await Context.Channel.SendTextAsync(echo + (mention ? Context.User.Mention : string.Empty));

    [SlashCommand("ping", "Pings the bot and returns its latency.")]
    public async Task GreetUserAsync()
        => await Context.Channel.SendTextAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!");

    // You can create command choices either by using the [Choice] attribute or by creating an enum. Every enum with 25 or less values will be registered as a multiple
    // choice option
    // [SlashCommand("choice_example", "Enums create choices")]
    // public async Task ChoiceExample(ExampleEnum input)
    //     => await RespondAsync(input.ToString());
}
