using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MasLazu.AspNet.EmailSender.Abstraction.Interfaces;
using MasLazu.AspNet.EmailSender.Abstraction.Models;
using MasLazu.AspNet.EmailSender.Gmail.Configurations;

namespace MasLazu.AspNet.EmailSender.Gmail.Services;

/// <summary>
/// Gmail implementation of the email sender.
/// </summary>
public class GmailEmailService : IEmailSender
{
    private readonly GmailConfiguration _options;
    private readonly ILogger<GmailEmailService> _logger;
    private readonly GmailService _gmailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GmailEmailService"/> class.
    /// </summary>
    /// <param name="options">The Gmail configuration options.</param>
    /// <param name="logger">The logger.</param>
    public GmailEmailService(IOptions<GmailConfiguration> options, ILogger<GmailEmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
        _gmailService = CreateGmailService();
    }

    /// <summary>
    /// Sends an email asynchronously using Gmail API.
    /// </summary>
    /// <param name="emailMessage">The email message to send.</param>
    /// <param name="renderer">The HTML renderer to use for template rendering (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(EmailMessage emailMessage, IHtmlRenderer? renderer = null)
    {
        try
        {
            _logger.LogInformation("Sending email via Gmail API to {Recipients}",
                string.Join(", ", emailMessage.To.Select(t => t.Email)));

            // Render the email body if renderer is provided
            string bodyContent = emailMessage.Body;
            if (renderer != null)
            {
                bodyContent = renderer.RenderEmail(emailMessage);
            }
            else if (!string.IsNullOrEmpty(emailMessage.BodyTemplate) && emailMessage.Model != null)
            {
                // Simple template rendering if no renderer provided
                bodyContent = RenderSimpleTemplate(emailMessage.BodyTemplate, emailMessage.Model);
            }

            // Create Gmail message
            Message gmailMessage = CreateGmailMessage(emailMessage, bodyContent);

            // Send the message
            UsersResource.MessagesResource.SendRequest request = _gmailService.Users.Messages.Send(gmailMessage, "me");
            Message response = await request.ExecuteAsync();

            _logger.LogInformation("Email sent successfully. Message ID: {MessageId}", response.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via Gmail API");
            throw new EmailSenderException("Failed to send email via Gmail API", ex);
        }
    }

    private GmailService CreateGmailService()
    {
        try
        {
            GoogleCredential credential;

            if (!string.IsNullOrEmpty(_options.ServiceAccountCredentialsJson))
            {
                // Use JSON string
                var credentialStream = new MemoryStream(Encoding.UTF8.GetBytes(_options.ServiceAccountCredentialsJson));
                credential = GoogleCredential.FromStream(credentialStream)
                    .CreateScoped(GmailService.Scope.GmailSend);
            }
            else if (!string.IsNullOrEmpty(_options.ServiceAccountCredentialsPath))
            {
                // Use file path
                credential = GoogleCredential.FromFile(_options.ServiceAccountCredentialsPath)
                    .CreateScoped(GmailService.Scope.GmailSend);
            }
            else
            {
                throw new InvalidOperationException("Either ServiceAccountCredentialsJson or ServiceAccountCredentialsPath must be provided");
            }

            // Impersonate user if specified
            if (!string.IsNullOrEmpty(_options.ImpersonateEmail))
            {
                credential = credential.CreateWithUser(_options.ImpersonateEmail);
            }

            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _options.ApplicationName,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Gmail service");
            throw new EmailSenderException("Failed to initialize Gmail service", ex);
        }
    }

    private Message CreateGmailMessage(EmailMessage emailMessage, string bodyContent)
    {
        var message = new StringBuilder();

        // Set sender
        EmailAddress fromAddress = emailMessage.From ??
            new EmailAddress(_options.DefaultFromEmail ?? throw new InvalidOperationException("From email address is required"),
                           _options.DefaultFromName);

        message.AppendLine($"From: {FormatEmailAddress(fromAddress)}");

        // Set recipients
        if (emailMessage.To.Any())
        {
            message.AppendLine($"To: {string.Join(", ", emailMessage.To.Select(FormatEmailAddress))}");
        }

        if (emailMessage.Cc.Any())
        {
            message.AppendLine($"Cc: {string.Join(", ", emailMessage.Cc.Select(FormatEmailAddress))}");
        }

        if (emailMessage.Bcc.Any())
        {
            message.AppendLine($"Bcc: {string.Join(", ", emailMessage.Bcc.Select(FormatEmailAddress))}");
        }

        // Set subject
        message.AppendLine($"Subject: {emailMessage.Subject}");
        message.AppendLine("MIME-Version: 1.0");
        message.AppendLine("Content-Type: text/html; charset=utf-8");
        message.AppendLine();

        // Set body
        message.AppendLine(bodyContent);

        // Encode message
        string rawMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message.ToString()))
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        return new Message { Raw = rawMessage };
    }

    private static string FormatEmailAddress(EmailAddress emailAddress)
    {
        return string.IsNullOrEmpty(emailAddress.Name)
            ? emailAddress.Email
            : $"{emailAddress.Name} <{emailAddress.Email}>";
    }

    private static string RenderSimpleTemplate(string template, object model)
    {
        System.Reflection.PropertyInfo[] properties = model.GetType().GetProperties();
        string result = template;

        foreach (System.Reflection.PropertyInfo property in properties)
        {
            string placeholder = $"{{{{{property.Name}}}}}";
            string value = property.GetValue(model)?.ToString() ?? string.Empty;
            result = result.Replace(placeholder, value);
        }

        return result;
    }

    /// <summary>
    /// Disposes the Gmail service.
    /// </summary>
    public void Dispose()
    {
        _gmailService?.Dispose();
    }
}
