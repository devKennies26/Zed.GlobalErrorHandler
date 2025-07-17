using Microsoft.AspNetCore.Builder;
using Zed.GlobalErrorHandler.Middleware;

namespace Zed.GlobalErrorHandler.Extensions;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to add global error handling middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="ErrorHandlingMiddleware"/> to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}