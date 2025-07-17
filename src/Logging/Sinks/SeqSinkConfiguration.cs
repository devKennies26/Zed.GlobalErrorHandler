using Serilog.Events;

namespace Zed.GlobalErrorHandler.Logging.Sinks;

/// <summary>
/// Configuration parameters for the Seq sink.
/// </summary>
public class SeqSinkConfiguration
{
    /// <summary>
    /// The Seq server URL.
    /// </summary>
    public string ServerUrl { get; set; } = default!;

    /// <summary>
    /// Optional API key for Seq authentication.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Minimum log event level to send to Seq.
    /// </summary>
    public LogEventLevel RestrictedToMinimumLevel { get; set; }
}