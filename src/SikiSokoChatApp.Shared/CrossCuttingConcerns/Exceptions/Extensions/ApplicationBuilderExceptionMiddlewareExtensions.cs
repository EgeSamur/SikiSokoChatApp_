using SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Middleware;
using Microsoft.AspNetCore.Builder;

namespace SikiSokoChatApp.Shared.CrossCuttingConcerns.Exceptions.Extensions;

/// <summary>
/// Provides extension methods for configuring custom exception middleware in the application.
/// </summary>
public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}