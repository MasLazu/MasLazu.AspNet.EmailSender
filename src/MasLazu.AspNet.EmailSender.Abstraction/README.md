# MasLazu.AspNet.EmailSender.Abstraction

The core abstraction layer for the MasLazu Email Sender library, providing interfaces, models, and contracts that enable a unified email sending experience across multiple providers.

## Overview

This package contains the foundational components that define how email sending works in the MasLazu ecosystem. It provides provider-agnostic interfaces and strongly-typed models that ensure consistency and type safety across all email provider implementations.

## 📦 Installation

```bash
dotnet add package MasLazu.AspNet.EmailSender.Abstraction
```

> **Note**: This package is automatically included when you install any provider package (Gmail, SendGrid, etc.). You typically don't need to install it separately unless you're building a custom provider.

## 🏗️ Architecture

### Core Interfaces

#### `IEmailSender`

The primary interface for sending emails across all providers.

```csharp
public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage emailMessage, IHtmlRenderer? renderer = null);
}
```

#### `IHtmlRenderer`

Interface for rendering beautiful HTML emails from templates and data.

```csharp
public interface IHtmlRenderer
{
    string RenderEmail(EmailMessage emailMessage);
}
```

### Core Models

| Model                  | Description                                                    |
| ---------------------- | -------------------------------------------------------------- |
| `EmailMessage`         | Complete email data including recipients, content, and options |
| `EmailMessageBuilder`  | Fluent builder for constructing email messages                 |
| `EmailAddress`         | Represents an email address with optional display name         |
| `EmailAttachment`      | File attachment with content and metadata                      |
| `EmailRenderOptions`   | Template rendering options (theme, colors, branding)           |
| `EmailSenderOptions`   | Base configuration options for providers                       |
| `EmailSenderException` | Specialized exception for email-related errors                 |

## 🛠️ Usage Examples

### Basic Email Construction

```csharp
var email = new EmailMessageBuilder()
    .From("sender@example.com", "Your App")
    .To("recipient@example.com", "John Doe")
    .Subject("Welcome to our service!")
    .Body("Thank you for joining us!")
    .Build();
```

### Rich HTML Email with Templates

```csharp
var email = new EmailMessageBuilder()
    .From("noreply@yourapp.com", "Your Company")
    .To("user@example.com", "Jane Smith")
    .Subject("Account Verification Required")
    .RenderOptions(new EmailRenderOptions
    {
        Theme = "VerificationCode",
        CompanyName = "Your Company",
        PrimaryColor = "#007bff",
        LogoUrl = "https://yourcompany.com/logo.png"
    })
    .Model(new
    {
        UserName = "Jane Smith",
        VerificationCode = "123456",
        ExpiryMinutes = 15
    })
    .Build();
```

### Email with Attachments

```csharp
var pdfContent = File.ReadAllBytes("report.pdf");
var imageContent = File.ReadAllBytes("chart.png");

var email = new EmailMessageBuilder()
    .From("reports@company.com", "Reporting System")
    .To("manager@company.com", "Department Manager")
    .Subject("Monthly Report")
    .Body("Please find the monthly report attached.")
    .AddAttachment("monthly-report.pdf", pdfContent, "application/pdf")
    .AddAttachment("performance-chart.png", imageContent, "image/png")
    .Build();
```

### Multiple Recipients

```csharp
var email = new EmailMessageBuilder()
    .From("newsletter@company.com", "Company Newsletter")
    .To("user1@example.com", "User One")
    .To("user2@example.com", "User Two")
    .Cc("manager@company.com", "Manager")
    .Bcc("audit@company.com")
    .Subject("Weekly Newsletter")
    .Body("This week's updates...")
    .Build();
```

## 📊 EmailMessage Properties

### Recipients

```csharp
public class EmailMessage
{
    public EmailAddress? From { get; set; }
    public List<EmailAddress> To { get; set; } = new();
    public List<EmailAddress>? Cc { get; set; }
    public List<EmailAddress>? Bcc { get; set; }
}
```

### Content

```csharp
public class EmailMessage
{
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? BodyTemplate { get; set; }
    public object? Model { get; set; }
    public Dictionary<string, string>? Parameters { get; set; }
}
```

### Attachments & Options

```csharp
public class EmailMessage
{
    public List<EmailAttachment>? Attachments { get; set; }
    public EmailRenderOptions? RenderOptions { get; set; }
}
```

## 🎨 EmailRenderOptions

Customize the appearance and branding of your email templates:

```csharp
public class EmailRenderOptions
{
    // Template Selection
    public string? Theme { get; set; } = "Default";

    // Branding
    public string? CompanyName { get; set; }
    public string? LogoUrl { get; set; }

    // Colors
    public string? PrimaryColor { get; set; } = "#007bff";
    public string? SecondaryColor { get; set; } = "#6c757d";
    public string? BackgroundColor { get; set; } = "#ffffff";
    public string? TextColor { get; set; } = "#333333";

    // Layout
    public string? FontFamily { get; set; } = "Arial, sans-serif";
    public bool ShowHeader { get; set; } = true;
    public bool ShowFooter { get; set; } = true;

    // Links
    public string? WebsiteUrl { get; set; }
    public string? UnsubscribeUrl { get; set; }
    public string? PrivacyPolicyUrl { get; set; }
    public string? TermsOfServiceUrl { get; set; }
}
```

## 🔗 Fluent Builder API

The `EmailMessageBuilder` provides a fluent, chainable API for constructing emails:

### Recipient Methods

```csharp
builder.From(email, name?)
       .To(email, name?)
       .Cc(email, name?)
       .Bcc(email, name?)
```

### Content Methods

```csharp
builder.Subject(subject)
       .Body(body)
       .BodyTemplate(template)
       .Model(data)
       .Parameters(dictionary)
```

### Customization Methods

```csharp
builder.RenderOptions(options)
       .AddAttachment(filename, content, contentType)
       .Priority(priority)
```

### Build Method

```csharp
EmailMessage email = builder.Build();
```

## 🔍 EmailAddress Model

Represents an email address with optional display name:

```csharp
public class EmailAddress
{
    public string Email { get; }
    public string? Name { get; }

    public EmailAddress(string email, string? name = null)
    {
        Email = email;
        Name = name;
    }
}

// Usage examples
var simple = new EmailAddress("user@example.com");
var withName = new EmailAddress("user@example.com", "John Doe");
```

## 📎 EmailAttachment Model

Represents a file attachment:

```csharp
public class EmailAttachment
{
    public string FileName { get; }
    public byte[] Content { get; }
    public string ContentType { get; }

    public EmailAttachment(string fileName, byte[] content, string contentType)
    {
        FileName = fileName;
        Content = content;
        ContentType = contentType;
    }
}

// Usage example
var attachment = new EmailAttachment(
    "report.pdf",
    pdfBytes,
    "application/pdf"
);
```

## ⚠️ EmailSenderException

Specialized exception for email-related errors:

```csharp
public class EmailSenderException : Exception
{
    public string? Provider { get; }
    public string? ErrorCode { get; }

    // Various constructors available
}

// Usage in providers
throw new EmailSenderException(
    "Failed to send email via SendGrid",
    innerException,
    provider: "SendGrid",
    errorCode: "UNAUTHORIZED"
);
```

## 🔌 Building Custom Providers

To build a custom email provider, implement the core interfaces:

### 1. Implement IEmailSender

```csharp
public class CustomEmailService : IEmailSender
{
    public async Task SendEmailAsync(EmailMessage emailMessage, IHtmlRenderer? renderer = null)
    {
        // Your custom implementation
        string htmlContent = renderer?.RenderEmail(emailMessage) ?? emailMessage.Body;

        // Send via your custom provider
        await SendViaCustomProvider(emailMessage, htmlContent);
    }
}
```

### 2. Implement IHtmlRenderer (Optional)

```csharp
public class CustomHtmlRenderer : IHtmlRenderer
{
    public string RenderEmail(EmailMessage emailMessage)
    {
        // Your custom template rendering logic
        return RenderTemplate(emailMessage);
    }
}
```

### 3. Create Extension Methods

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomEmailSender(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CustomConfiguration>(
            configuration.GetSection("Custom"));

        services.AddScoped<IEmailSender, CustomEmailService>();
        services.AddScoped<IHtmlRenderer, CustomHtmlRenderer>();

        return services;
    }
}
```

## 🎯 Design Principles

### Provider Agnostic

The abstraction layer ensures that switching between email providers requires no changes to your application code.

### Type Safety

All models use nullable reference types and strong typing to catch errors at compile time.

### Fluent API

The builder pattern provides an intuitive, discoverable API for constructing emails.

### Extensibility

Interfaces and base classes are designed to be easily extended for custom implementations.

### Performance

Models are lightweight and optimized for serialization and memory usage.

## 📝 Validation

The abstraction layer includes built-in validation:

```csharp
// EmailMessageBuilder validates during construction
var email = new EmailMessageBuilder()
    .From("") // Throws ArgumentException
    .To("invalid-email") // Throws ArgumentException
    .Build();

// EmailAddress validates email format
var address = new EmailAddress("not-an-email"); // Throws ArgumentException
```

## 🚀 Integration with DI

All components are designed for dependency injection:

```csharp
public class EmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly IHtmlRenderer _htmlRenderer;

    public EmailController(IEmailSender emailSender, IHtmlRenderer htmlRenderer)
    {
        _emailSender = emailSender;
        _htmlRenderer = htmlRenderer;
    }

    public async Task<IActionResult> SendWelcomeEmail(string userEmail)
    {
        var email = new EmailMessageBuilder()
            .From("welcome@app.com", "Welcome Team")
            .To(userEmail)
            .Subject("Welcome!")
            .RenderOptions(new EmailRenderOptions { Theme = "Default" })
            .Model(new { UserEmail = userEmail })
            .Build();

        await _emailSender.SendEmailAsync(email, _htmlRenderer);
        return Ok();
    }
}
```

## 📚 Related Packages

- **[MasLazu.AspNet.EmailSender.Gmail](../MasLazu.AspNet.EmailSender.Gmail/README.md)** - Gmail API implementation
- **[MasLazu.AspNet.EmailSender.SendGrid](../MasLazu.AspNet.EmailSender.SendGrid/README.md)** - SendGrid API implementation

## 🤝 Contributing

This is the foundation package for the entire email sending ecosystem. When contributing:

1. Maintain backward compatibility
2. Add comprehensive XML documentation
3. Include unit tests for all new functionality
4. Follow the established patterns and naming conventions

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.
