using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using MasLazu.AspNet.EmailSender.Abstraction.Interfaces;
using MasLazu.AspNet.EmailSender.SendGrid.Configurations;
using System.Text.RegularExpressions;
using SGEmailAddress = SendGrid.Helpers.Mail.EmailAddress;
using EmailAddress = MasLazu.AspNet.EmailSender.Abstraction.Models.EmailAddress;
using EmailMessage = MasLazu.AspNet.EmailSender.Abstraction.Models.EmailMessage;
using EmailAttachment = MasLazu.AspNet.EmailSender.Abstraction.Models.EmailAttachment;
using MasLazu.AspNet.EmailSender.Abstraction.Models;

namespace MasLazu.AspNet.EmailSender.SendGrid.Services;

public class SendGridEmailService : IEmailSender
{
    private readonly SendGridConfiguration _config;
    private readonly ISendGridClient _sendGridClient;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(
        IOptions<SendGridConfiguration> config,
        ISendGridClient sendGridClient,
        ILogger<SendGridEmailService> logger)
    {
        _config = config.Value;
        _sendGridClient = sendGridClient;
        _logger = logger;

        _config.Validate();
    }

    public async Task SendEmailAsync(EmailMessage emailMessage, IHtmlRenderer? htmlRenderer = null)
    {
        try
        {
            SendGridMessage sendGridMessage = CreateSendGridMessage(emailMessage, htmlRenderer);

            _logger.LogInformation("Sending email to {Recipients} with subject: {Subject}",
                string.Join(", ", emailMessage.To.Select(t => t.Email)), emailMessage.Subject);

            Response response = await _sendGridClient.SendEmailAsync(sendGridMessage);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully to {Recipients}",
                    string.Join(", ", emailMessage.To.Select(t => t.Email)));
            }
            else
            {
                string errorBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("Failed to send email. Status: {StatusCode}, Body: {Body}",
                    response.StatusCode, errorBody);
                throw new InvalidOperationException($"Failed to send email. Status: {response.StatusCode}, Body: {errorBody}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Recipients}",
                string.Join(", ", emailMessage.To.Select(t => t.Email)));
            throw;
        }
    }

    private SendGridMessage CreateSendGridMessage(EmailMessage emailMessage, IHtmlRenderer? htmlRenderer)
    {
        var message = new SendGridMessage();

        // From
        string fromEmail = emailMessage.From?.Email ?? _config.DefaultFromEmail;
        string fromName = emailMessage.From?.Name ?? _config.DefaultFromName;
        message.SetFrom(new SGEmailAddress(fromEmail, fromName));

        // To
        foreach (EmailAddress recipient in emailMessage.To)
        {
            message.AddTo(new SGEmailAddress(recipient.Email, recipient.Name));
        }

        // CC
        if (emailMessage.Cc?.Any() == true)
        {
            foreach (EmailAddress cc in emailMessage.Cc)
            {
                message.AddCc(new SGEmailAddress(cc.Email, cc.Name));
            }
        }

        // BCC
        if (emailMessage.Bcc?.Any() == true)
        {
            foreach (EmailAddress bcc in emailMessage.Bcc)
            {
                message.AddBcc(new SGEmailAddress(bcc.Email, bcc.Name));
            }
        }

        // Subject
        message.SetSubject(emailMessage.Subject);

        // Body content
        string? htmlContent = null;
        string? plainTextContent = null;

        if (htmlRenderer != null && (!string.IsNullOrEmpty(emailMessage.BodyTemplate) || emailMessage.Model != null))
        {
            // Render HTML using the renderer
            htmlContent = htmlRenderer.RenderEmail(emailMessage);

            // Generate plain text version from HTML (basic conversion)
            plainTextContent = StripHtmlTags(htmlContent);
        }
        else if (!string.IsNullOrEmpty(emailMessage.Body))
        {
            // Use the body as-is
            if (IsHtml(emailMessage.Body))
            {
                htmlContent = emailMessage.Body;
                plainTextContent = StripHtmlTags(emailMessage.Body);
            }
            else
            {
                plainTextContent = emailMessage.Body;
            }
        }

        // Set content
        if (!string.IsNullOrEmpty(htmlContent))
        {
            message.AddContent(MimeType.Html, htmlContent);
        }

        if (!string.IsNullOrEmpty(plainTextContent))
        {
            message.AddContent(MimeType.Text, plainTextContent);
        }

        // Attachments
        if (emailMessage.Attachments?.Any() == true)
        {
            foreach (EmailAttachment attachment in emailMessage.Attachments)
            {
                Attachment sendGridAttachment = new()
                {
                    Content = Convert.ToBase64String(attachment.Content),
                    Filename = attachment.FileName,
                    Type = attachment.ContentType,
                    Disposition = "attachment"
                };
                message.AddAttachment(sendGridAttachment);
            }
        }

        // Tracking settings
        message.SetClickTracking(_config.EnableClickTracking, _config.EnableClickTracking);
        message.SetOpenTracking(_config.EnableOpenTracking);
        message.SetSubscriptionTracking(_config.EnableSubscriptionTracking);

        // Sandbox mode
        if (_config.SandboxMode)
        {
            message.SetSandBoxMode(true);
        }

        return message;
    }

    private static bool IsHtml(string content)
    {
        return content.Contains('<') && content.Contains('>') &&
               (content.Contains("<html>", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<div>", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<p>", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<h1>", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<h2>", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("<h3>", StringComparison.OrdinalIgnoreCase));
    }

    private static string StripHtmlTags(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return string.Empty;
        }

        // Basic HTML to text conversion
        string text = html;

        // Replace line breaks
        text = text.Replace("<br>", "\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("<br/>", "\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("<br />", "\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</p>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</div>", "\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h1>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h2>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h3>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h4>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h5>", "\n\n", StringComparison.OrdinalIgnoreCase);
        text = text.Replace("</h6>", "\n\n", StringComparison.OrdinalIgnoreCase);

        // Remove all HTML tags
        text = Regex.Replace(text, "<.*?>", string.Empty);

        // Decode HTML entities
        text = System.Net.WebUtility.HtmlDecode(text);

        // Clean up multiple newlines
        text = Regex.Replace(text, @"\n\s*\n", "\n\n");

        return text.Trim();
    }
}
