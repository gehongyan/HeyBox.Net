using System.Diagnostics;

namespace HeyBox.Rest;

/// <summary>
///     表示一个消息内基于的附件。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Attachment : IAttachment
{
    /// <inheritdoc />
    public AttachmentType Type { get; }

    /// <inheritdoc />
    public string Url { get; }

    /// <inheritdoc />
    public string? Filename { get; }

    /// <inheritdoc />
    public int? Size { get; }

    /// <inheritdoc />
    public string? FileType { get; }

    /// <inheritdoc />
    public int? Width { get; }

    /// <inheritdoc />
    public int? Height { get; }

    internal Attachment(AttachmentType type, string url, string? filename,
        int? size = null, string? fileType = null, int? width = null, int? height = null)
    {
        Type = type;
        Url = url;
        Filename = filename;
        Size = size;
        FileType = fileType;
        Width = width;
        Height = height;
    }

    /// <inheritdoc cref="HeyBox.Rest.Attachment.Filename" />
    /// <returns> 此附件的文件名。 </returns>
    public override string? ToString() => Filename;

    private string DebuggerDisplay => $"{Filename}{(Size.HasValue ? $" ({Size} bytes)" : "")}";
}
