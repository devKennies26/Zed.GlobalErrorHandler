using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Zed.GlobalErrorHandler.Logging.Sinks;

namespace Zed.GlobalErrorHandler.Logging;

/// <summary>
/// Extension methods for <see cref="ILoggingBuilder"/> to configure Serilog global logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds and configures Serilog as the global logger with options for file and Seq sinks.
    /// </summary>
    /// <param name="builder">The logging builder to extend.</param>
    /// <param name="configure">An optional action to configure <see cref="LoggingOptions"/>.</param>
    /// <returns>The updated <see cref="ILoggingBuilder"/>.</returns>
    public static ILoggingBuilder AddSerilogGlobalLogger(
        this ILoggingBuilder builder,
        Action<LoggingOptions>? configure = null)
    {
        LoggingOptions options = new LoggingOptions();
        configure?.Invoke(options);

        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Is(options.MinimumLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext();

        if (options.HasAnySinkConfigured)
        {
            if (options.EnableFile)
            {
                loggerConfig.WriteTo.File(
                    path: options.FilePath!,
                    rollingInterval: options.RollingInterval,
                    retainedFileCountLimit: options.RetainedFileCountLimit,
                    restrictedToMinimumLevel: options.MinimumLevel);
            }

            if (options.EnableSeq)
            {
                SeqSinkConfiguration seqConfig = new SeqSinkConfiguration
                {
                    ServerUrl = options.SeqUrl!,
                    ApiKey = options.SeqApiKey,
                    RestrictedToMinimumLevel = options.MinimumLevel
                };

                if (!string.IsNullOrEmpty(seqConfig.ApiKey))
                {
                    loggerConfig.WriteTo.Seq(
                        serverUrl: seqConfig.ServerUrl,
                        apiKey: seqConfig.ApiKey,
                        restrictedToMinimumLevel: seqConfig.RestrictedToMinimumLevel);
                }
                else
                {
                    loggerConfig.WriteTo.Seq(
                        serverUrl: seqConfig.ServerUrl,
                        restrictedToMinimumLevel: seqConfig.RestrictedToMinimumLevel);
                }
            }
        }
        else
        {
            loggerConfig = new LoggerConfiguration().MinimumLevel.Fatal();
        }

        Log.Logger = loggerConfig.CreateLogger();

        builder.ClearProviders();
        builder.AddSerilog(Log.Logger, dispose: true);

        return builder;
    }
}