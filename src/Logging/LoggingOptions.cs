using System.Reflection;
using Serilog;
using Serilog.Events;

namespace Zed.GlobalErrorHandler.Logging;

/// <summary>
/// Configuration options for setting up logging sinks and levels.
/// Supports file and Seq sinks with customizable parameters.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// Indicates whether file logging is enabled.
    /// </summary>
    internal bool EnableFile { get; private set; }

    /// <summary>
    /// Indicates whether Seq logging is enabled.
    /// </summary>
    internal bool EnableSeq { get; private set; }

    /// <summary>
    /// The file path template for rolling file logs.
    /// </summary>
    internal string? FilePath { get; private set; }

    /// <summary>
    /// The rolling interval for file logs.
    /// Defaults to daily rolling.
    /// </summary>
    internal RollingInterval RollingInterval { get; private set; } = RollingInterval.Day;

    /// <summary>
    /// The maximum number of retained rolling files.
    /// </summary>
    internal int? RetainedFileCountLimit { get; private set; } = 7;

    /// <summary>
    /// The URL of the Seq server.
    /// </summary>
    internal string? SeqUrl { get; private set; }

    /// <summary>
    /// The API key used for authenticating with Seq.
    /// Optional.
    /// </summary>
    internal string? SeqApiKey { get; private set; }

    /// <summary>
    /// The minimum log event level for capturing logs.
    /// Defaults to Information.
    /// </summary>
    internal LogEventLevel MinimumLevel { get; private set; } = LogEventLevel.Information;

    /// <summary>
    /// The application name used in logs.
    /// Defaults to entry assembly name or "UnknownApp".
    /// </summary>
    internal string ApplicationName { get; private set; } = GetDefaultApplicationName();

    /// <summary>
    /// Indicates if any sink (file or Seq) has been enabled.
    /// </summary>
    internal bool HasAnySinkConfigured => EnableFile || EnableSeq;

    /// <summary>
    /// Enables and configures file logging sink.
    /// </summary>
    /// <param name="path">The log file path template (default: "logs/log-.txt").</param>
    /// <param name="interval">The rolling interval for log files (default: daily).</param>
    /// <param name="retainCount">The number of log files to retain (default: 7).</param>
    /// <returns>The current <see cref="LoggingOptions"/> instance.</returns>
    public LoggingOptions WithFile(
        string path = "logs/log-.txt",
        RollingInterval interval = RollingInterval.Day,
        int? retainCount = 7)
    {
        EnableFile = true;
        FilePath = path;
        RollingInterval = interval;
        RetainedFileCountLimit = retainCount;
        return this;
    }

    /// <summary>
    /// Enables and configures Seq logging sink.
    /// </summary>
    /// <param name="url">The Seq server URL (default: "http://localhost:5341").</param>
    /// <param name="apiKey">Optional API key for authenticating with Seq.</param>
    /// <returns>The current <see cref="LoggingOptions"/> instance.</returns>
    public LoggingOptions WithSeq(
        string? url = null,
        string? apiKey = null)
    {
        EnableSeq = true;
        SeqUrl = url ?? "http://localhost:5341";
        SeqApiKey = apiKey;
        return this;
    }

    /// <summary>
    /// Sets the minimum log event level.
    /// </summary>
    /// <param name="level">The minimum level of log events to capture.</param>
    /// <returns>The current <see cref="LoggingOptions"/> instance.</returns>
    public LoggingOptions WithMinimumLevel(LogEventLevel level)
    {
        MinimumLevel = level;
        return this;
    }

    /// <summary>
    /// Sets the application name used in logs.
    /// </summary>
    /// <param name="name">The application name.</param>
    /// <returns>The current <see cref="LoggingOptions"/> instance.</returns>
    public LoggingOptions WithApplicationName(string name)
    {
        ApplicationName = name;
        return this;
    }

    /// <summary>
    /// Retrieves the default application name from the entry assembly.
    /// Returns "UnknownApp" if entry assembly is not found.
    /// </summary>
    private static string GetDefaultApplicationName()
    {
        Assembly? entry = Assembly.GetEntryAssembly();
        return entry?.GetName().Name ?? "UnknownApp";
    }
}