using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zed.GlobalErrorHandler.Mappers;

namespace Zed.GlobalErrorHandler.Middleware;

public class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger, 
    ExceptionToStatusCodeMapper mapper) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var statusCode = mapper.Map(ex);
            var message = statusCode == mapper.DefaultStatusCode 
                ? mapper.DefaultErrorMessage 
                : ex.Message;
            await HandleExceptionAsync(context, statusCode, ex, message);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, int statusCode, Exception ex, string message)
    {
        logger.LogError(ex, ex.Message);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode
        });

        return context.Response.WriteAsync(response);
    }
}