using HeyBox.Interactions;
using HeyBox.Net.Samples.SimpleBot.Data;

namespace HeyBox.Net.Samples.SimpleBot.Modules;

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

    [SlashCommand("test")]
    public async Task VariablesAsync(
        [Summary("文本")] string text,
        [Summary("单选")] ItemEnum selection,
        [Summary("用户")] IUser user,
        [Summary("整数")] int integer,
        [Summary("布尔值")] bool boolean) =>
        await ReplyTextAsync(
            $"文本: {text}\n" +
            $"单选: {selection}\n" +
            $"用户: {user.Mention}\n" +
            $"整数: {integer}\n" +
            $"布尔值: {boolean}");

    [SlashCommand("echo", runMode: RunMode.Async)]
    public async Task Echo(
        [Summary("内容")] string echo,
        [Summary("提及")] bool mention = false,
        [Summary("回复")] bool reply = false)
    {
        IUserMessage message = await ReplyTextAsync(
            $"{(mention ? MentionUtils.MentionUser(Context.UserId) : string.Empty)} {echo}",
            reply: reply);
        await Task.Delay(TimeSpan.FromSeconds(1));
        await message.ModifyAsync(x => x.Content += " (edited)");
        await Task.Delay(TimeSpan.FromSeconds(1));
        await message.DeleteAsync();
    }

    [SlashCommand("ping")]
    public async Task GreetUserAsync() =>
        await ReplyTextAsync(text: $"\ud83c\udfd3 It took me {Context.Client.Latency}ms to respond to you!");

    // You can create command choices either by using the [Choice] attribute or by creating an enum. Every enum with 25 or less values will be registered as a multiple
    // choice option
    // [SlashCommand("choice_example", "Enums create choices")]
    // public async Task ChoiceExample(ExampleEnum input) =>
    //     await RespondAsync(input.ToString());
}
