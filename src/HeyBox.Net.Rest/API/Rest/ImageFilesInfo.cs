using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal class ImageFilesInfo
{
    [JsonPropertyName("img_files_info")]
    public required ImageFileInfo[] FilesInfo { get; set; }
}
