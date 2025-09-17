namespace MasLazu.AspNet.EmailSender.Gmail.Configurations;

/// <summary>
/// Configuration options for Gmail email sending.
/// </summary>
public class GmailConfiguration
{
    /// <summary>
    /// Gets or sets the path to the Gmail service account credentials JSON file.
    /// </summary>
    public string? ServiceAccountCredentialsPath { get; set; }

    /// <summary>
    /// Gets or sets the Gmail service account credentials as a JSON string.
    /// </summary>
    public string? ServiceAccountCredentialsJson { get; set; }

    /// <summary>
    /// Gets or sets the email address to impersonate (for domain-wide delegation).
    /// </summary>
    public string? ImpersonateEmail { get; set; }

    /// <summary>
    /// Gets or sets the application name for Gmail API requests.
    /// </summary>
    public string ApplicationName { get; set; } = "MasLazu Email Sender";

    /// <summary>
    /// Gets or sets the default sender email address.
    /// </summary>
    public string? DefaultFromEmail { get; set; }

    /// <summary>
    /// Gets or sets the default sender display name.
    /// </summary>
    public string? DefaultFromName { get; set; }
}
