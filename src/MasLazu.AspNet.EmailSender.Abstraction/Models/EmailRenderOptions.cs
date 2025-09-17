using System.Collections.Generic;

namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Configuration for email HTML rendering themes and styles.
/// </summary>
public class EmailRenderOptions
{
    /// <summary>
    /// Gets or sets the email theme (e.g., "modern", "classic", "minimal").
    /// </summary>
    public string Theme { get; set; } = "modern";

    /// <summary>
    /// Gets or sets the primary color for the email theme.
    /// </summary>
    public string PrimaryColor { get; set; } = "#007bff";

    /// <summary>
    /// Gets or sets the secondary color for the email theme.
    /// </summary>
    public string SecondaryColor { get; set; } = "#6c757d";

    /// <summary>
    /// Gets or sets the background color for the email.
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";

    /// <summary>
    /// Gets or sets the text color for the email.
    /// </summary>
    public string TextColor { get; set; } = "#333333";

    /// <summary>
    /// Gets or sets the font family for the email.
    /// </summary>
    public string FontFamily { get; set; } = "Arial, sans-serif";

    /// <summary>
    /// Gets or sets the company logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the footer text.
    /// </summary>
    public string? FooterText { get; set; }

    /// <summary>
    /// Gets or sets whether to include social media links.
    /// </summary>
    public bool IncludeSocialLinks { get; set; } = false;

    /// <summary>
    /// Gets or sets the social media links.
    /// </summary>
    public Dictionary<string, string> SocialLinks { get; set; } = new();
}
