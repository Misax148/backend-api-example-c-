namespace MovieApi.Core.src.Domain.Exceptions;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime TimeStamp { get; set; }
    public List<ValidationError>? Details { get; set; }
}
