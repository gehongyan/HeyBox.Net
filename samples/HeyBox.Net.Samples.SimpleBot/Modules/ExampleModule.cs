using HeyBox.Interactions;
using HeyBox.Net.Samples.SimpleBot.Data;
using HeyBox.WebSocket;

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
            $"""
            文本: {text}
            单选: {selection}
            用户: {user.Mention}
            整数: {integer}
            布尔值: {boolean}
            """);

    [SlashCommand("attachment")]
    public async Task AttachmentAsync(
        [Summary("图片1")] IAttachment image1,
        [Summary("图片2")] IAttachment image2,
        [Summary("文件1")] IAttachment file1,
        [Summary("文件2")] IAttachment file2)
    {
        await ReplyTextAsync(
            $"""
            图片1: {image1.Url}
            图片2: {image2.Url}
            文件1: {file1.Url}
            文件2: {file2.Url}
            """);
    }

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
            .AddModule<HeaderModuleBuilder>(x => x.WithContent(Format.Bold("Header"), true))
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
                .AddImage(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/2.png"))
                .AddImage(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/3.png"))
                .AddImage(y => y.WithUrl("https://cdn.max-c.com/stickers/heyboxgirl/4.png")))
            .AddModule<ButtonGroupModuleBuilder>(x => x
                .AddButton(new ButtonNodeBuilder("Primary", ButtonEvent.Server, "primary", ButtonTheme.Primary))
                .AddButton(new ButtonNodeBuilder("Link", ButtonEvent.LinkTo, "https://www.baidu.com", ButtonTheme.Danger))
                .AddButton(new ButtonNodeBuilder("Success", ButtonEvent.Server, "success", ButtonTheme.Success)))
            .AddModule<CountdownModuleBuilder>(x => x
                .WithMode(CountdownMode.Default)
                .WithEndTime(DateTimeOffset.UtcNow.AddDays(1)));
        await ReplyCardAsync(builder.Build());
    }

    [SlashCommand("stop")]
    [RequireUser(12345678)]
    public async Task StopAsync()
    {
        if (Context.User is not SocketRoomUser roomUser) return;
        IReadOnlyCollection<SocketRole> roles = roomUser.Room.Roles;
        IEnumerable<RoomPermissions> permissions = roomUser.Roles.Select(x => x.Permissions);
        RoomPermissions roomPermissions = roomUser.RoomPermissions;
        if (Context.Channel is IRoomChannel roomChannel)
        {
            ChannelPermissions channelPermissions = roomUser.GetPermissions(roomChannel);
        }
        await ReplyTextAsync("Goodbye!");
        Environment.Exit(0);
    }

    // You can create command choices either by using the [Choice] attribute or by creating an enum. Every enum with 25 or less values will be registered as a multiple
    // choice option
    // [SlashCommand("choice_example", "Enums create choices")]
    // public async Task ChoiceExample(ExampleEnum input) =>
    //     await RespondAsync(input.ToString());
}
