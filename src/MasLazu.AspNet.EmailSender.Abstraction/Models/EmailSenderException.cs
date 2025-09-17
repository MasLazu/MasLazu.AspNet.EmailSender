using System;

namespace MasLazu.AspNet.EmailSender.Abstraction.Models;

/// <summary>
/// Exception thrown when an error occurs while sending an email.
/// </summary>
public class EmailSenderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSenderException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public EmailSenderException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSenderException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EmailSenderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
