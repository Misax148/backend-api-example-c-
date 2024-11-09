using System.Net;

namespace MovieApi.Core.src.Domain.Exceptions;

public abstract class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; }
    public DateTime TimeStamp { get; }

    public ApiException(HttpStatusCode statusCode, string message, string errorCode)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        TimeStamp = DateTime.UtcNow;
    }
}
