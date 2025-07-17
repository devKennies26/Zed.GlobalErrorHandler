# Zed.GlobalErrorHandler

[![NuGet](https://img.shields.io/nuget/v/Zed.GlobalErrorHandler.svg)](https://www.nuget.org/packages/Zed.GlobalErrorHandler)

Zed.GlobalErrorHandler is a simple global exception handling middleware for ASP.NET Core. It allows you to map custom exceptions to specific HTTP status codes, log exceptions, and return consistent JSON error responses.

## Features

- Centralized global exception handling
- Custom exception-to-status-code mapping
- Configurable default status code and error message
- Built-in logging support
- Minimal setup

## Usage

1. Install via NuGet:

```bash
dotnet add package Zed.GlobalErrorHandler
```

2. Register in `Program.cs`

```csharp
builder.Services.AddGlobalErrorHandler(opt =>
{
    // Add your project exceptions and status codes
    opt.Add(typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized);
    opt.Add(typeof(NotFoundException), StatusCodes.Status404NotFound);
},
// If you want to override the default status code and error message (used for unregistered exceptions):
defaultStatusCode: 400, 
defaultErrorMessage: "Custom default error message.");
```

 3. Add global error handler to the pipeline

```csharp
app.UseGlobalErrorHandler();
```

## How It Works

Internally, the middleware uses the provided mapping to determine the correct HTTP status code for an exception. If the exception type is not mapped, it uses the default status code and message.

```json
{
  "error": "Your exception message or default error message.",
  "statusCode": 400
}
```

Logged errors include exception details and stack trace.

## Advanced logging with Serilog
You can also enable logging using Serilog with support for both rolling file logs and Seq server:

```csharp
builder.Services.AddGlobalErrorHandler(opt =>
{
    // Register your exceptions and HTTP status codes
})
.AddLogging(logging =>
{
    logging.AddSerilogGlobalLogger(options =>
    {
        // Enable file logging (daily rolling)
        options.WithFile("logs/log-.txt", RollingInterval.Day, 7);

        // Enable Seq logging (make sure Seq is running at the given URL)
        options.WithSeq("http://localhost:5341");

        // Optional: Set application name and minimum log level
        options.WithApplicationName("YourProjectName");
        options.WithMinimumLevel(LogEventLevel.Information);
    });
});
```

