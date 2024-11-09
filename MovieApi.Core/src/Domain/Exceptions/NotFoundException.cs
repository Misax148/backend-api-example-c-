using System.Net;

namespace MovieApi.Core.src.Domain.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message) 
        : base(HttpStatusCode.NotFound, message, "NOT_FOUND")
    {
    }
}
