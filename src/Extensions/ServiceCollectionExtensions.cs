using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Zed.GlobalErrorHandler.Mappers;
using Zed.GlobalErrorHandler.Middleware;

namespace Zed.GlobalErrorHandler.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlobalErrorHandler(this IServiceCollection services, 
        Action<Dictionary<Type, int>> configure, 
        int defaultStatusCode = StatusCodes.Status500InternalServerError, 
        string defaultErrorMessage = "Something went wrong.")
    {
        var mapping = new Dictionary<Type, int>();
        configure.Invoke(mapping);
        
        services.AddSingleton<ExceptionToStatusCodeMapper>(new ExceptionToStatusCodeMapper(mapping, 
            defaultStatusCode, defaultErrorMessage));
        services.AddTransient<ErrorHandlingMiddleware>();
        
        return services;
    }
}