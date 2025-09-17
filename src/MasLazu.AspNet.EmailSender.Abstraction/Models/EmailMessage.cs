using System.Collections.Generic;

namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Represents an email message.
/// </summary>
public class EmailMessage
{
    /// <summary>
    /// Gets or sets the sender's email address.
    /// </summary>
    public EmailAddress From { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of recipient email addresses.
    /// </summary>
    public List<EmailAddress> To { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of CC recipient email addresses.
    /// </summary>
    public List<EmailAddress> Cc { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of BCC recipient email addresses.
    /// </summary>
    public List<EmailAddress> Bcc { get; set; } = new();

    /// <summary>
    /// Gets or sets the subject of the email.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the body of the email.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HTML template for the email body.
    /// </summary>
    public string? BodyTemplate { get; set; }

    /// <summary>
    /// Gets or sets the model data for template rendering.
    /// </summary>
    public object? Model { get; set; }

    /// <summary>
    /// Gets or sets the rendering options for styling the email.
    /// </summary>
    public EmailRenderOptions? RenderOptions { get; set; }

    /// <summary>
    /// Gets or sets the list of attachments.
    /// </summary>
    public List<EmailAttachment> Attachments { get; set; } = new();
}
