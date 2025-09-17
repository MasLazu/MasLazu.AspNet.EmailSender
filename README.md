# MasLazu.AspNet.EmailSender

A comprehensive, multi-provider email sending library for ASP.NET applications with beautiful HTML templates and Razor rendering support.

## 📦 Packages

| Package                                    | Description                 | Status   |
| ------------------------------------------ | --------------------------- | -------- |
| **MasLazu.AspNet.EmailSender.Abstraction** | Core interfaces and models  | ✅ Ready |
| **MasLazu.AspNet.EmailSender.Gmail**       | Gmail API implementation    | ✅ Ready |
| **MasLazu.AspNet.EmailSender.SendGrid**    | SendGrid API implementation | ✅ Ready |

## 🚀 Features

### Core Features

- **Multi-Provider Support**: Gmail API and SendGrid implementations
- **Beautiful HTML Templates**: Pre-built Razor templates for professional emails
- **Template Engine**: Razor-based rendering with model binding
- **Type Safety**: Strongly typed with nullable reference types
- **Dependency Injection**: Full DI container support
- **Async/Await**: Modern async patterns throughout

### Email Templates

- **Professional Templates**: Default, Dark, and Minimal themes
- **Verification Codes**: Specialized security-focused templates
- **Mobile Responsive**: All templates work on mobile devices
- **Customizable**: Company branding, colors, and logos
- **Accessibility**: WCAG compliant templates

### Provider-Specific Features

#### Gmail API

- Service account authentication
- Domain-wide delegation support
- OAuth 2.0 integration
- Advanced Gmail features

#### SendGrid

- API key authentication
- Click and open tracking
- Subscription management
- Sandbox mode for testing

## 📋 Quick Start

### 1. Choose Your Provider

#### Gmail Setup

```bash
dotnet add package MasLazu.AspNet.EmailSender.Gmail
```

```csharp
// Program.cs or Startup.cs
services.AddGmailEmailSender(configuration);
```

#### SendGrid Setup

```bash
dotnet add package MasLazu.AspNet.EmailSender.SendGrid
```

```csharp
// Program.cs or Startup.cs
services.AddSendGridEmailSender(configuration);
```

### 2. Configure Your Provider

#### Gmail Configuration

```json
{
  "Gmail": {
    "ServiceAccountCredentialsPath": "path/to/credentials.json",
    "ImpersonateEmail": "sender@yourdomain.com",
    "ApplicationName": "Your App Name",
    "DefaultFromEmail": "noreply@yourdomain.com",
    "DefaultFromName": "Your Company"
  }
}
```

#### SendGrid Configuration

```json
{
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key",
    "DefaultFromEmail": "noreply@yourdomain.com",
    "DefaultFromName": "Your Company",
    "SandboxMode": false,
    "EnableClickTracking": true,
    "EnableOpenTracking": true
  }
}
```

### 3. Send Your First Email

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

    [HttpPost("send-welcome")]
    public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
    {
        var email = new EmailMessageBuilder()
            .From("welcome@yourapp.com", "Your App")
            .To(request.UserEmail, request.UserName)
            .Subject("Welcome to Your App!")
            .RenderOptions(new EmailRenderOptions
            {
                Theme = "Default",
                CompanyName = "Your Company",
                PrimaryColor = "#007bff",
                LogoUrl = "https://yourapp.com/logo.png"
            })
            .Model(new
            {
                UserName = request.UserName,
                AppName = "Your App",
                GetStartedUrl = "https://yourapp.com/getting-started"
            })
            .Build();

        await _emailSender.SendEmailAsync(email, _htmlRenderer);
        return Ok();
    }
}
```

## 🔐 Verification Code Templates

Send beautiful, secure verification codes with specialized templates:

```csharp
[HttpPost("send-verification")]
public async Task<IActionResult> SendVerificationCode([FromBody] VerificationRequest request)
{
    var verificationCode = GenerateSecureCode(); // Your implementation

    var email = new EmailMessageBuilder()
        .From("security@yourapp.com", "Your App Security")
        .To(request.Email)
        .Subject("🔐 Verify Your Account")
        .RenderOptions(new EmailRenderOptions
        {
            Theme = "VerificationCode", // Uses specialized template
            CompanyName = "Your Company",
            PrimaryColor = "#28a745"
        })
        .Model(new
        {
            VerificationCode = verificationCode,
            UserName = request.UserName,
            ExpiryMinutes = 15
        })
        .Build();

    await _emailSender.SendEmailAsync(email, _htmlRenderer);
    return Ok();
}
```

## 🎨 Available Templates

### General Purpose Templates

- **Default**: Modern, professional design with customizable branding
- **Dark**: Contemporary dark theme with gradient effects
- **Minimal**: Clean, minimalist design for simple communications

### Verification Templates

- **VerificationCode**: Security-focused with prominent code display
- **ModernVerification**: Contemporary design with animations

### Template Features

- ✅ **Security Warnings**: Built-in security tips and reminders
- ✅ **Copy to Clipboard**: One-click code copying functionality
- ✅ **Mobile Responsive**: Perfect rendering on all devices
- ✅ **Accessibility**: WCAG 2.1 compliant design
- ✅ **Customizable**: Company colors, logos, and branding

## 🛠️ Advanced Usage

### Multiple Providers

You can use multiple providers in the same application:

```csharp
// Use different providers for different purposes
services.AddGmailEmailSender(gmailConfig);
services.AddSendGridEmailSender(sendGridConfig);

// Register specific implementations
services.AddScoped<IGmailEmailService, GmailEmailService>();
services.AddScoped<ISendGridEmailService, SendGridEmailService>();
```

### Custom Templates

Create your own Razor templates:

```html
@model EmailMessage
<!DOCTYPE html>
<html>
  <head>
    <title>@Model.Subject</title>
    <style>
      /* Your custom styles */
    </style>
  </head>
  <body>
    <h1>Hello @Model.Model.UserName!</h1>
    <p>@Model.Body</p>
  </body>
</html>
```

### Attachments

Send emails with file attachments:

```csharp
var email = new EmailMessageBuilder()
    .From("sender@example.com")
    .To("recipient@example.com")
    .Subject("Document Attached")
    .Body("Please find the attached document.")
    .AddAttachment("report.pdf", pdfBytes, "application/pdf")
    .AddAttachment("data.xlsx", excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
    .Build();

await emailSender.SendEmailAsync(email);
```

## 📖 Documentation

- [Gmail Provider Documentation](src/MasLazu.AspNet.EmailSender.Gmail/README.md)
- [SendGrid Provider Documentation](src/MasLazu.AspNet.EmailSender.SendGrid/README.md)
- [Template Customization Guide](docs/templates.md)
- [API Reference](docs/api.md)

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- 📧 Email: support@maslazu.com
- 🐛 Issues: [GitHub Issues](https://github.com/maslazu/aspnet-emailsender/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/maslazu/aspnet-emailsender/discussions)

---

Made with ❤️ by the MasLazu team
