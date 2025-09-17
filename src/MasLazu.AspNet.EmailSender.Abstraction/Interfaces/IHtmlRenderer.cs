using MasLazu.AspNet.EmailSender.Abstraction.Models;

namespace MasLazu.AspNet.EmailSender.Abstraction.Interfaces;

/// <summary>
/// Interface for rendering beautiful HTML emails from email message data.
/// </summary>
public interface IHtmlRenderer
{
    /// <summary>
    /// Renders a beautiful HTML email from the email message parameters.
    /// Takes the subject, body, template, model, and render options to create
    /// a professionally styled HTML email.
    /// </summary>
    /// <param name="emailMessage">The email message containing all parameters.</param>
    /// <returns>The rendered HTML string for the email body.</returns>
    string RenderEmail(EmailMessage emailMessage);
}
