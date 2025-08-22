using System.Text.Json.Serialization;

namespace HeyBox.API;

internal class RoomAutoMod
{
    [JsonPropertyName("filter_word_list")]
    public AutoModFilterWord[]? FilterWordList { get; set; }

    [JsonPropertyName("filter_urls")]
    public AutoModFilterUrl? FilterUrls { get; set; }

    [JsonPropertyName("filter_qrcode")]
    public AutoModFilterBase? FilterQrcode { get; set; }
}