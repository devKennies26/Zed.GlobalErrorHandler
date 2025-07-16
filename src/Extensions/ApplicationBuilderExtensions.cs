using Microsoft.AspNetCore.Builder;
using Zed.GlobalErrorHandler.Middleware;

namespace Zed.GlobalErrorHandler.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}