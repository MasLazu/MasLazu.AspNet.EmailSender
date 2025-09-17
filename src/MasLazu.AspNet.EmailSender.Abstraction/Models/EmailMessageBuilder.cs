using System.Collections.Generic;

namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Builder for creating <see cref="EmailMessage"/> instances.
/// </summary>
public class EmailMessageBuilder
{
    private readonly EmailMessage _emailMessage = new();

    /// <summary>
    /// Sets the sender's email address.
    /// </summary>
    /// <param name="email">The sender's email address.</param>
    /// <param name="name">The sender's display name (optional).</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder From(string email, string? name = null)
    {
        _emailMessage.From = new EmailAddress(email, name);
        return this;
    }

    /// <summary>
    /// Adds a recipient email address.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="name">The recipient's display name (optional).</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder To(string email, string? name = null)
    {
        _emailMessage.To.Add(new EmailAddress(email, name));
        return this;
    }

    /// <summary>
    /// Adds multiple recipient email addresses.
    /// </summary>
    /// <param name="emails">The list of recipient email addresses.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder To(IEnumerable<string> emails)
    {
        foreach (string email in emails)
        {
            _emailMessage.To.Add(new EmailAddress(email));
        }
        return this;
    }

    /// <summary>
    /// Adds a CC recipient email address.
    /// </summary>
    /// <param name="email">The CC recipient's email address.</param>
    /// <param name="name">The CC recipient's display name (optional).</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Cc(string email, string? name = null)
    {
        _emailMessage.Cc.Add(new EmailAddress(email, name));
        return this;
    }

    /// <summary>
    /// Adds a BCC recipient email address.
    /// </summary>
    /// <param name="email">The BCC recipient's email address.</param>
    /// <param name="name">The BCC recipient's display name (optional).</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Bcc(string email, string? name = null)
    {
        _emailMessage.Bcc.Add(new EmailAddress(email, name));
        return this;
    }

    /// <summary>
    /// Sets the subject of the email.
    /// </summary>
    /// <param name="subject">The subject.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Subject(string subject)
    {
        _emailMessage.Subject = subject;
        return this;
    }

    /// <summary>
    /// Sets the body of the email.
    /// </summary>
    /// <param name="body">The body content.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Body(string body)
    {
        _emailMessage.Body = body;
        return this;
    }

    /// <summary>
    /// Sets the HTML template for the email body.
    /// </summary>
    /// <param name="template">The HTML template.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder BodyTemplate(string template)
    {
        _emailMessage.BodyTemplate = template;
        return this;
    }

    /// <summary>
    /// Sets the model data for template rendering.
    /// </summary>
    /// <param name="model">The model data.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Model(object model)
    {
        _emailMessage.Model = model;
        return this;
    }

    /// <summary>
    /// Sets the rendering options for styling the email.
    /// </summary>
    /// <param name="renderOptions">The render options.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder RenderOptions(EmailRenderOptions renderOptions)
    {
        _emailMessage.RenderOptions = renderOptions;
        return this;
    }

    /// <summary>
    /// Adds an attachment to the email.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="content">The content of the attachment.</param>
    /// <param name="contentType">The MIME type of the attachment.</param>
    /// <returns>The builder instance.</returns>
    public EmailMessageBuilder Attach(string fileName, byte[] content, string contentType)
    {
        _emailMessage.Attachments.Add(new EmailAttachment(fileName, content, contentType));
        return this;
    }

    /// <summary>
    /// Builds the email message.
    /// </summary>
    /// <returns>The constructed <see cref="EmailMessage"/>.</returns>
    public EmailMessage Build()
    {
        return _emailMessage;
    }
}
