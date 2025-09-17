namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Represents an email attachment.
/// </summary>
public class EmailAttachment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAttachment"/> class.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="content">The content of the attachment as a byte array.</param>
    /// <param name="contentType">The MIME type of the attachment.</param>
    public EmailAttachment(string fileName, byte[] content, string contentType)
    {
        FileName = fileName;
        Content = content;
        ContentType = contentType;
    }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the content of the attachment.
    /// </summary>
    public byte[] Content { get; }

    /// <summary>
    /// Gets the MIME type of the attachment.
    /// </summary>
    public string ContentType { get; }
}
