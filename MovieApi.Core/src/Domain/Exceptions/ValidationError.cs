namespace MovieApi.Core.src.Domain.Exceptions;

public class ValidationError
{
    public string? PropertyName { get; set; }
    public string? ErrorMessage { get; set; }
}
