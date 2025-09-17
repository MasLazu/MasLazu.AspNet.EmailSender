using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid;
using MasLazu.AspNet.EmailSender.Abstraction.Interfaces;
using MasLazu.AspNet.EmailSender.SendGrid.Configurations;
using MasLazu.AspNet.EmailSender.SendGrid.Services;
using MasLazu.AspNet.EmailSender.SendGrid.Renderers;

namespace MasLazu.AspNet.EmailSender.SendGrid.Extensions;

/// <summary>
/// Extension methods for configuring SendGrid email services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds SendGrid email sender services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="sectionName">The configuration section name (default: "SendGrid").</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSendGridEmailSender(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = SendGridConfiguration.SectionName)
    {
        services.Configure<SendGridConfiguration>(configuration.GetSection(sectionName));
        services.AddSingleton<ISendGridClient>(provider =>
        {
            SendGridConfiguration options = provider.GetRequiredService<IOptions<SendGridConfiguration>>().Value;
            return new SendGridClient(options.ApiKey);
        });

        services.AddScoped<IEmailSender, SendGridEmailService>();
        services.AddScoped<IHtmlRenderer, SendGridHtmlRenderer>();

        services.AddMvcCore()
            .AddRazorViewEngine()
            .AddRazorRuntimeCompilation();

        return services;
    }

    /// <summary>
    /// Adds SendGrid email sender services to the service collection with configuration action.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure SendGrid options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSendGridEmailSender(
        this IServiceCollection services,
        Action<SendGridConfiguration> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddSingleton<ISendGridClient>(provider =>
        {
            SendGridConfiguration options = provider.GetRequiredService<IOptions<SendGridConfiguration>>().Value;
            return new SendGridClient(options.ApiKey);
        });

        services.AddScoped<IEmailSender, SendGridEmailService>();
        services.AddScoped<IHtmlRenderer, SendGridHtmlRenderer>();

        services.AddMvcCore()
            .AddRazorViewEngine()
            .AddRazorRuntimeCompilation();

        return services;
    }

    /// <summary>
    /// Adds SendGrid email sender services to the service collection with API key.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="apiKey">The SendGrid API key.</param>
    /// <param name="defaultFromEmail">The default from email address.</param>
    /// <param name="defaultFromName">The default from name (optional).</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSendGridEmailSender(
        this IServiceCollection services,
        string apiKey,
        string defaultFromEmail,
        string? defaultFromName = null)
    {
        return services.AddSendGridEmailSender(options =>
        {
            options.ApiKey = apiKey;
            options.DefaultFromEmail = defaultFromEmail;
            options.DefaultFromName = defaultFromName ?? string.Empty;
        });
    }
}
