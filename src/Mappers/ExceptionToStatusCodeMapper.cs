namespace Zed.GlobalErrorHandler.Mappers;

/// <summary>
/// Maps exception types to corresponding HTTP status codes.
/// </summary>
public class ExceptionToStatusCodeMapper
{
    private readonly Dictionary<Type, int> exceptionStatusCodes;

    /// <summary>
    /// Gets the default HTTP status code used for unmapped exceptions.
    /// </summary>
    public int DefaultStatusCode { get; private set; }

    /// <summary>
    /// Gets the default error message returned for unmapped exceptions.
    /// </summary>
    public string DefaultErrorMessage { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="ExceptionToStatusCodeMapper"/>.
    /// </summary>
    /// <param name="exceptionStatusCodes">
    /// A dictionary mapping exception types to HTTP status codes.
    /// </param>
    /// <param name="defaultStatusCode">The default HTTP status code.</param>
    /// <param name="defaultErrorMessage">The default error message.</param>
    public ExceptionToStatusCodeMapper(Dictionary<Type, int> exceptionStatusCodes, int defaultStatusCode,
        string defaultErrorMessage)
    {
        this.exceptionStatusCodes = exceptionStatusCodes;
        DefaultStatusCode = defaultStatusCode;
        DefaultErrorMessage = defaultErrorMessage;
    }

    /// <summary>
    /// Maps an exception instance to its corresponding HTTP status code.
    /// Returns the default status code if the exception type is not mapped.
    /// </summary>
    /// <param name="exception">The exception instance to map.</param>
    /// <returns>The HTTP status code associated with the exception type.</returns>
    public int Map(Exception exception)
    {
        return exceptionStatusCodes.TryGetValue(exception.GetType(), out int code)
            ? code
            : DefaultStatusCode;
    }
}