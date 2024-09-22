using HeyBox;
using HeyBox.Interactions;
using HeyBox.Net.Samples.SimpleBot;
using HeyBox.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(null);
builder.Services.AddSingleton(new HeyBoxSocketConfig
{
    LogLevel = LogSeverity.Debug
});
builder.Services.AddSingleton<HeyBoxSocketClient>();
builder.Services.AddSingleton(new InteractionServiceConfig
{
    LogLevel = LogSeverity.Debug
});
builder.Services.AddSingleton<InteractionService>();
builder.Services.AddSingleton<InteractionHandler>();
IHost app = builder.Build();

HeyBoxSocketClient client = app.Services.GetRequiredService<HeyBoxSocketClient>();
client.Log += message =>
{
    Console.WriteLine(message);
    return Task.CompletedTask;
};
await app.Services.GetRequiredService<InteractionHandler>().InitializeAsync();
await client.LoginAsync(TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);

// HeyBoxSocketClient client = new(new HeyBoxSocketConfig
// {
//     LogLevel = LogSeverity.Debug
// });
// client.Log += message =>
// {
//     Console.WriteLine(message);
//     return Task.CompletedTask;
// };
// client.SlashCommandExecuted += command =>
// {
//     return Task.CompletedTask;
// };
// await client.LoginAsync(TokenType.BotToken, "");
// await client.StartAsync();
// await Task.Delay(Timeout.Infinite);

// SocketRoom room = client.GetRoom(0);
// SocketTextChannel textChannel = room.GetTextChannel(0);
//
// await textChannel.SendTextAsync(textChannel.Mention);
//
// await textChannel.SendFileAsync(@"D:\OneDrive\Pictures\wc3YcYyqeE0n00o4.jpg");
//
// await using FileStream fileStream = File.OpenRead(@"D:\OneDrive\Pictures\wc3YcYyqeE0n00o4.jpg");
// await textChannel.SendFileAsync(fileStream, "wc3YcYyqeE0n00o4.jpg", imageSize: new Size(100, 50));
//
// using FileAttachment attachment1 = new(
//     new Uri("https://chat.max-c.com/attachments/2024-09-18/1836323544355573760_GAmVNMcLQr.jpg"),
//     "1836323544355573760_GAmVNMcLQr.jpg",
//     imageSize: new Size(100, 50));
// await textChannel.SendFileAsync(attachment1);
//
// Uri uri = new("https://chat.max-c.com/attachments/2024-09-18/1836323544355573760_GAmVNMcLQr.jpg");
// using FileAttachment attachment2 = new(uri, "1836323544355573760_GAmVNMcLQr.jpg", imageSize: new Size(100, 50));
// await textChannel.SendTextAsync($"TextGraphicMixed {Format.Image(attachment2)} {MentionUtils.MentionEveryone} {MentionUtils.MentionHere}", imageFileInfos: [attachment]);

