# MasLazu.AspNet.EmailSender.Gmail

Gmail implementation for the MasLazu Email Sender library.

## Features

- Send emails using Gmail API
- Service account authentication
- Domain-wide delegation support
- Beautiful HTML email rendering
- Template support with model binding
- Customizable email themes and styling

## Configuration

### Using appsettings.json

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

### Using code configuration

```csharp
services.AddGmailEmailSender(options =>
{
    options.ServiceAccountCredentialsJson = "{ ... }";
    options.ImpersonateEmail = "sender@yourdomain.com";
    options.ApplicationName = "Your App Name";
    options.DefaultFromEmail = "noreply@yourdomain.com";
    options.DefaultFromName = "Your Company";
});
```

## Usage

```csharp
// Register services
services.AddGmailEmailSender(configuration);

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

## Gmail API Setup

1. Create a Google Cloud Project
2. Enable the Gmail API
3. Create a Service Account
4. Download the credentials JSON file
5. (Optional) Set up domain-wide delegation for impersonation
