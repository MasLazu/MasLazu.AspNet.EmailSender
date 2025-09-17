using System.Threading.Tasks;
using MasLazu.AspNet.EmailSender.Abstraction.Models;

namespace MasLazu.AspNet.EmailSender.Abstraction.Interfaces;

/// <summary>
/// Interface for sending emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="emailMessage">The email message to send.</param>
    /// <param name="renderer">The HTML renderer to use for template rendering (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(EmailMessage emailMessage, IHtmlRenderer? renderer = null);
}
