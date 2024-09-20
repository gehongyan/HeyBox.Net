using System.Drawing;
using HeyBox;
using HeyBox.WebSocket;

HeyBoxSocketClient client = new(new HeyBoxSocketConfig
{
    LogLevel = LogSeverity.Debug
});
client.Log += message =>
{
    Console.WriteLine(message);
    return Task.CompletedTask;
};
client.SlashCommandExecuted += command =>
{
    return Task.CompletedTask;
};
await client.LoginAsync(TokenType.BotToken, string.Empty);
await client.StartAsync();
await Task.Delay(Timeout.Infinite);

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

