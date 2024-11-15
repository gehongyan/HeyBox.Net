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

    [SlashCommand("card")]
    public async Task CardAsync()
    {
        ICardBuilder builder = new CardBuilder()
            .AddModule<HeaderModuleBuilder>(x => x.WithText(Format.Bold("Header"), true))
            .AddModule<DividerModuleBuilder>(x => x.WithText("Divider"))
            .AddModule<SectionModuleBuilder>(x => x
                .AddNode<MarkdownNodeBuilder>(y => y.WithText(Format.Bold("Section")))
                .AddNode<MarkdownNodeBuilder>(y => y.WithText(Format.Italics("Center")))
                .AddNode<PlainTextNodeBuilder>(y => y.WithText("Text")))
            .AddModule<SectionModuleBuilder>(x => x
                .AddNode<MarkdownNodeBuilder>(y => y.WithText(Format.Bold("Section")))
                .AddNode<ImageNodeBuilder>(y => y
                    .WithSize(ImageSize.Medium)
                    .WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/1.png")))
            .AddModule<ImagesModuleBuilder>(x => x
                .AddNode(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/2.png"))
                .AddNode(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/3.png"))
                .AddNode(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/4.png")))
            .AddModule<ButtonGroupModuleBuilder>(x => x
                .AddNode(new ButtonNodeBuilder("Primary", ButtonEvent.Server, "primary", ButtonTheme.Primary))
                .AddNode(new ButtonNodeBuilder("Danger", ButtonEvent.Server, "danger", ButtonTheme.Danger))
                .AddNode(new ButtonNodeBuilder("Success", ButtonEvent.Server, "success", ButtonTheme.Success)))
            .AddModule<CountdownModuleBuilder>(x => x
                .WithMode(CountdownMode.Default)
                .WithEndTime(DateTimeOffset.UtcNow.AddDays(1)));
        await ReplyCardAsync(builder.Build());
    }

    [SlashCommand("stop")]
    [RequireUser(12345678)]
    public async Task StopAsync()
    {
        await ReplyTextAsync("Goodbye!");
        Environment.Exit(0);
    }

    // You can create command choices either by using the [Choice] attribute or by creating an enum. Every enum with 25 or less values will be registered as a multiple
    // choice option
    // [SlashCommand("choice_example", "Enums create choices")]
    // public async Task ChoiceExample(ExampleEnum input) =>
    //     await RespondAsync(input.ToString());
}
