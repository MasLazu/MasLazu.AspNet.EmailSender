# MasLazu.AspNet.EmailSender.SendGrid

SendGrid implementation for the MasLazu Email Sender library.

## Features

- Send emails using SendGrid API
- API key authentication
- Beautiful HTML email rendering
- Template support with model binding
- Customizable email themes and styling
- Tracking capabilities (click, open, subscription)
- Sandbox mode for testing

## Configuration

### Using appsettings.json

```json
{
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key",
    "DefaultFromEmail": "noreply@yourdomain.com",
    "DefaultFromName": "Your Company",
    "SandboxMode": false,
    "EnableClickTracking": true,
    "EnableOpenTracking": true,
    "EnableSubscriptionTracking": false
  }
}
```

### Using code configuration

```csharp
services.AddSendGridEmailSender(options =>
{
    options.ApiKey = "your-sendgrid-api-key";
    options.DefaultFromEmail = "noreply@yourdomain.com";
    options.DefaultFromName = "Your Company";
    options.SandboxMode = false;
    options.EnableClickTracking = true;
    options.EnableOpenTracking = true;
    options.EnableSubscriptionTracking = false;
});
```

### Simple configuration

```csharp
services.AddSendGridEmailSender(
    apiKey: "your-sendgrid-api-key",
    defaultFromEmail: "noreply@yourdomain.com",
    defaultFromName: "Your Company"
);
```

## Usage

```csharp
// Register services
services.AddSendGridEmailSender(configuration);

// Send email
var email = new EmailMessageBuilder()
    .From("sender@example.com", "Sender Name")
    .To("recipient@example.com")
    .Subject("Welcome!")
    .BodyTemplate("<h1>Hello {{Name}}!</h1>")
    .Model(new { Name = "John" })
    .RenderOptions(new EmailRenderOptions
    {
        Theme = "modern",
        PrimaryColor = "#007bff",
        CompanyName = "My Company",
        LogoUrl = "https://example.com/logo.png"
    })
    .Build();

await emailSender.SendEmailAsync(email);
```

## Email Templates

This library includes specialized Razor templates for various email types, including beautiful verification code templates.

### Available Templates

#### General Templates

- **Default.cshtml**: Modern, professional email template with customizable colors and branding
- **Dark.cshtml**: Dark theme template with gradient effects and contemporary styling
- **Minimal.cshtml**: Clean, minimal template following modern design principles

#### Verification Code Templates

- **VerificationCode.cshtml**: Professional, security-focused template with prominent code display
- **ModernVerification.cshtml**: Contemporary template with animations and premium styling

### Template Features

#### Security Features

- ✅ Prominent security warnings and tips
- ✅ Never share code reminders
- ✅ Expiry time notifications
- ✅ Professional security styling

#### User Experience

- ✅ One-click copy to clipboard functionality
- ✅ Mobile-responsive design
- ✅ Clear, readable code display
- ✅ Accessibility considerations

#### Customization

- ✅ Company branding support
- ✅ Logo integration
- ✅ Color theme customization
- ✅ Custom expiry times

### Verification Code Usage

#### Basic Verification Email

```csharp
var email = new EmailMessageBuilder()
    .From("noreply@yourapp.com", "Your App")
    .To("user@example.com")
    .Subject("Verify Your Account")
    .Body("Please use the verification code below to verify your account.")
    .RenderOptions(new EmailRenderOptions
    {
        Theme = "VerificationCode", // Uses VerificationCode.cshtml
        CompanyName = "Your Company",
        PrimaryColor = "#3b82f6"
    })
    .Parameters(new Dictionary<string, string>
    {
        ["VerificationCode"] = "123456",
        ["ExpiryMinutes"] = "15",
        ["UserName"] = "John Doe"
    })
    .Build();

await emailSender.SendEmailAsync(email, htmlRenderer);
```

#### Modern Verification Style

```csharp
var email = new EmailMessageBuilder()
    .From("security@yourapp.com", "Your App Security")
    .To("user@example.com")
    .Subject("🔐 Account Verification Required")
    .RenderOptions(new EmailRenderOptions
    {
        Theme = "ModernVerification", // Uses ModernVerification.cshtml
        CompanyName = "Your Company",
        LogoUrl = "https://yourapp.com/logo.png"
    })
    .Model(new
    {
        VerificationCode = "987654",
        UserName = "Jane Smith"
    })
    .Build();

await emailSender.SendEmailAsync(email, htmlRenderer);
```

### Template Selection

The renderer automatically determines which template to use based on:

1. **Explicit Theme**: `RenderOptions.Theme = "VerificationCode"`
2. **Template Name**: `BodyTemplate = "ModernVerification"`
3. **Fallback**: Uses "Default" template if specified template not found

### Code Generation Best Practices

```csharp
// Generate secure 6-digit codes
var code = new Random().Next(100000, 999999).ToString();

// Or use more secure generation
var rng = new RNGCryptoServiceProvider();
var bytes = new byte[4];
rng.GetBytes(bytes);
var code = (Math.Abs(BitConverter.ToInt32(bytes, 0)) % 900000 + 100000).ToString();
```

## SendGrid Setup

1. Sign up for a SendGrid account
2. Create an API key with "Mail Send" permissions
3. (Optional) Set up domain authentication for better deliverability
4. (Optional) Configure IP warming and dedicated IP if needed
5. Use sandbox mode for testing without sending actual emails

## Advanced Features

### Tracking

SendGrid provides built-in tracking capabilities:

- **Click Tracking**: Track when recipients click links in your emails
- **Open Tracking**: Track when recipients open your emails
- **Subscription Tracking**: Allow recipients to unsubscribe

### Sandbox Mode

Enable sandbox mode for testing:

```csharp
services.AddSendGridEmailSender(options =>
{
    options.ApiKey = "your-api-key";
    options.DefaultFromEmail = "test@example.com";
    options.SandboxMode = true; // Emails won't be sent
});
```

### Attachments

Send emails with attachments:

```csharp
var email = new EmailMessageBuilder()
    .From("sender@example.com")
    .To("recipient@example.com")
    .Subject("Document Attached")
    .Body("Please find the attached document.")
    .AddAttachment("document.pdf", pdfBytes, "application/pdf")
    .Build();

await emailSender.SendEmailAsync(email);
```
