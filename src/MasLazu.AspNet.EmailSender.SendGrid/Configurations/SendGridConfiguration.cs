namespace MasLazu.AspNet.EmailSender.SendGrid.Configurations;

public class SendGridConfiguration
{
    public const string SectionName = "SendGrid";

    /// <summary>
    /// SendGrid API Key for authentication
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Default from email address
    /// </summary>
    public string DefaultFromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Default from name
    /// </summary>
    public string DefaultFromName { get; set; } = string.Empty;

    /// <summary>
    /// Enable sandbox mode for testing (emails won't be sent)
    /// </summary>
    public bool SandboxMode { get; set; } = false;

    /// <summary>
    /// Custom tracking settings
    /// </summary>
    public bool EnableClickTracking { get; set; } = true;

    /// <summary>
    /// Enable open tracking
    /// </summary>
    public bool EnableOpenTracking { get; set; } = true;

    /// <summary>
    /// Enable subscription tracking
    /// </summary>
    public bool EnableSubscriptionTracking { get; set; } = false;

    /// <summary>
    /// Validate required configuration values
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException("SendGrid API Key is required");
        }

        if (string.IsNullOrWhiteSpace(DefaultFromEmail))
        {
            throw new InvalidOperationException("Default from email is required");
        }
    }
}
