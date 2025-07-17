using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Zed.GlobalErrorHandler.Mappers;
using Zed.GlobalErrorHandler.Middleware;

namespace Zed.GlobalErrorHandler.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register global error handling services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers global error handling services, including exception-to-status-code mappings and middleware.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configure">
    /// An action to configure exception-to-HTTP-status-code mappings.
    /// The key is the exception type, the value is the HTTP status code to return.
    /// </param>
    /// <param name="defaultStatusCode">
    /// The default HTTP status code to use when an exception type is not mapped.
    /// Defaults to 500 (Internal Server Error).
    /// </param>
    /// <param name="defaultErrorMessage">
    /// The default error message to return for unmapped exceptions.
    /// </param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGlobalErrorHandler(this IServiceCollection services,
        Action<Dictionary<Type, int>> configure,
        int defaultStatusCode = StatusCodes.Status500InternalServerError,
        string defaultErrorMessage = "Something went wrong.")
    {
        Dictionary<Type, int> mapping = new Dictionary<Type, int>();
        configure.Invoke(mapping);

        services.AddSingleton<ExceptionToStatusCodeMapper>(new ExceptionToStatusCodeMapper(mapping,
            defaultStatusCode, defaultErrorMessage));
        services.AddTransient<ErrorHandlingMiddleware>();

        return services;
    }
}