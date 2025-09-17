using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.EmailSender.Abstraction.Interfaces;
using MasLazu.AspNet.EmailSender.Abstraction.Models;

namespace MasLazu.AspNet.EmailSender.Gmail.Renderers;

/// <summary>
/// A Razor HTML renderer for Gmail emails that creates beautiful email layouts using Razor templates.
/// </summary>
public class GmailHtmlRenderer : IHtmlRenderer
{
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GmailHtmlRenderer"/> class.
    /// </summary>
    /// <param name="razorViewEngine">The Razor view engine.</param>
    /// <param name="tempDataProvider">The temp data provider.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public GmailHtmlRenderer(
        IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    /// Renders a beautiful HTML email from the email message parameters using Razor templates.
    /// </summary>
    /// <param name="emailMessage">The email message containing all parameters.</param>
    /// <returns>The rendered HTML string for the email body.</returns>
    public string RenderEmail(EmailMessage emailMessage)
    {
        string viewName = DetermineViewName(emailMessage);

        try
        {
            return RenderViewAsync(viewName, emailMessage).GetAwaiter().GetResult();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Could not find view"))
        {
            // Fall back to default view if specified view is not found
            if (viewName != "Default")
            {
                return RenderViewAsync("Default", emailMessage).GetAwaiter().GetResult();
            }
            throw;
        }
    }

    /// <summary>
    /// Determines the view name to use for rendering the email.
    /// </summary>
    /// <param name="emailMessage">The email message.</param>
    /// <returns>The view name to use.</returns>
    private static string DetermineViewName(EmailMessage emailMessage)
    {
        // If BodyTemplate is specified and looks like a view name (no HTML tags), use it
        if (!string.IsNullOrEmpty(emailMessage.BodyTemplate) &&
            !emailMessage.BodyTemplate.Contains("<") &&
            !emailMessage.BodyTemplate.Contains(">"))
        {
            return emailMessage.BodyTemplate;
        }

        // Use theme from render options, defaulting to "Default"
        string theme = emailMessage.RenderOptions?.Theme ?? "Default";
        return char.ToUpperInvariant(theme[0]) + theme[1..].ToLowerInvariant();
    }

    /// <summary>
    /// Renders a Razor view with the email message as the model.
    /// </summary>
    /// <param name="viewName">The name of the Razor view.</param>
    /// <param name="emailMessage">The email message to use as the model.</param>
    /// <returns>The rendered HTML string.</returns>
    private async Task<string> RenderViewAsync(string viewName, EmailMessage emailMessage)
    {
        ActionContext actionContext = GetActionContext();
        ViewEngineResult viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if (!viewResult.Success)
        {
            throw new InvalidOperationException($"Could not find view '{viewName}'. Searched locations: {string.Join(", ", viewResult.SearchedLocations)}");
        }

        IView view = viewResult.View;
        using var output = new StringWriter();

        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = emailMessage
            },
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            output,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);
        return output.ToString();
    }

    /// <summary>
    /// Creates an ActionContext for rendering Razor views.
    /// </summary>
    /// <returns>A configured ActionContext.</returns>
    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        return new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());
    }
}

