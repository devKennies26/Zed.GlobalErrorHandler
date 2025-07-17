using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Zed.GlobalErrorHandler.Mappers;

namespace Zed.GlobalErrorHandler.Middleware;

/// <summary>
/// Middleware that catches exceptions thrown in the HTTP request pipeline,
/// maps them to HTTP status codes, logs the error, and returns a JSON error response.
/// </summary>
public class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger,
    ExceptionToStatusCodeMapper mapper) : IMiddleware
{
    /// <summary>
    /// Invokes the middleware to handle exceptions from downstream middleware.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            int statusCode = mapper.Map(ex);
            string message = statusCode == mapper.DefaultStatusCode
                ? mapper.DefaultErrorMessage
                : ex.Message;
            await HandleExceptionAsync(context, statusCode, ex, message);
        }
    }

    /// <summary>
    /// Handles the exception by setting the response status code and content,
    /// and logging the error.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="ex">The exception that was caught.</param>
    /// <param name="message">The error message to include in the response.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private Task HandleExceptionAsync(HttpContext context, int statusCode, Exception ex, string message)
    {
        logger.LogError(ex, ex.Message);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        string response = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode
        });

        return context.Response.WriteAsync(response);
    }
}