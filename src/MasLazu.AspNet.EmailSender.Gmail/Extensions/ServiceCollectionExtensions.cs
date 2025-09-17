using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MasLazu.AspNet.EmailSender.Abstraction.Interfaces;
using MasLazu.AspNet.EmailSender.Gmail.Configurations;
using MasLazu.AspNet.EmailSender.Gmail.Services;
using MasLazu.AspNet.EmailSender.Gmail.Renderers;

namespace MasLazu.AspNet.EmailSender.Gmail.Extensions;

/// <summary>
/// Extension methods for registering Gmail email sender services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Gmail email sender to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddGmailEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GmailConfiguration>(configuration.GetSection("Gmail"));

        // Add MVC services required for Razor
        services.AddMvc();
        services.AddRazorPages();

        services.AddScoped<IEmailSender, GmailEmailService>();
        services.AddScoped<IHtmlRenderer, GmailHtmlRenderer>();
        return services;
    }

    /// <summary>
    /// Adds Gmail email sender to the service collection with custom configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">The configuration action.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddGmailEmailSender(this IServiceCollection services, Action<GmailConfiguration> configureOptions)
    {
        services.Configure(configureOptions);

        // Add MVC services required for Razor
        services.AddMvc();
        services.AddRazorPages();

        services.AddScoped<IEmailSender, GmailEmailService>();
        services.AddScoped<IHtmlRenderer, GmailHtmlRenderer>();
        return services;
    }
}
