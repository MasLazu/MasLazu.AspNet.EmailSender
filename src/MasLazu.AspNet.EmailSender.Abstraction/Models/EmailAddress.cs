namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Represents an email address.
/// </summary>
public class EmailAddress
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAddress"/> class.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <param name="name">The display name (optional).</param>
    public EmailAddress(string email, string? name = null)
    {
        Email = email;
        Name = name;
    }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    public string? Name { get; }
}
