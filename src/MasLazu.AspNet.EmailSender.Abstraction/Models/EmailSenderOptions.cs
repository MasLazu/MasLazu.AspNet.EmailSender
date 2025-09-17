namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Configuration options for email sending.
/// </summary>
public class EmailSenderOptions
{
    /// <summary>
    /// Gets or sets the SMTP server host.
    /// </summary>
    public string? SmtpHost { get; set; }

    /// <summary>
    /// Gets or sets the SMTP server port.
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Gets or sets whether to use SSL.
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the default sender email address.
    /// </summary>
    public string? DefaultFromEmail { get; set; }

    /// <summary>
    /// Gets or sets the default sender display name.
    /// </summary>
    public string? DefaultFromName { get; set; }
}
