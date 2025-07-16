using Microsoft.AspNetCore.Http;

namespace Zed.GlobalErrorHandler.Mappers;

public class ExceptionToStatusCodeMapper
{
    private readonly Dictionary<Type, int> exceptionStatusCodes;
    public int DefaultStatusCode { get; private set; }
    public string DefaultErrorMessage { get; private set; }

    public ExceptionToStatusCodeMapper(Dictionary<Type, int> exceptionStatusCodes, int defaultStatusCode, string defaultErrorMessage)
    {
        this.exceptionStatusCodes = exceptionStatusCodes;
        DefaultStatusCode = defaultStatusCode;
        DefaultErrorMessage = defaultErrorMessage;
    }

    public int Map(Exception exception)
    {
        return exceptionStatusCodes.TryGetValue(exception.GetType(), out var code)
            ? code
            : DefaultStatusCode;
    }
}