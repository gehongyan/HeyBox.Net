using System.Drawing;
using HeyBox;
using HeyBox.Rest;

HeyBoxRestClient client = new(new HeyBoxRestConfig
{
    LogLevel = LogSeverity.Debug
});
client.Log += message =>
{
    Console.WriteLine(message);
    return Task.CompletedTask;
};
await client.LoginAsync(TokenType.BotToken, string.Empty);
RestRoom room = await client.GetRoomAsync(0);
RestTextChannel textChannel = await room.GetTextChannelAsync(0);

// await textChannel.SendTextAsync(textChannel.Mention);

// await textChannel.SendFileAsync(@"D:\OneDrive\Pictures\wc3YcYyqeE0n00o4.jpg", imageSize: new Size(100, 50));

// await using FileStream fileStream = File.OpenRead(@"D:\OneDrive\Pictures\wc3YcYyqeE0n00o4.jpg");
// await textChannel.SendFileAsync(fileStream, "wc3YcYyqeE0n00o4.jpg", imageSize: new Size(100, 50));

// using FileAttachment attachment = new(
//     new Uri("https://chat.max-c.com/attachments/2024-09-18/1836323544355573760_GAmVNMcLQr.jpg"),
//     "1836323544355573760_GAmVNMcLQr.jpg",
//     imageSize: new Size(100, 50));
// await textChannel.SendFileAsync(attachment);

// Uri uri = new("https://chat.max-c.com/attachments/2024-09-18/1836323544355573760_GAmVNMcLQr.jpg");
// using FileAttachment attachment = new(uri, "1836323544355573760_GAmVNMcLQr.jpg", imageSize: new Size(100, 50));
// await textChannel.SendTextAsync($"TextGraphicMixed {Format.Image(attachment)} {MentionUtils.MentionEveryone} {MentionUtils.MentionHere}", imageFileInfos: [attachment]);

