namespace MovieApi.Core.src.Application.DTO.Movie;

public record MovieSimpleDto(
    Guid Id,
    string? Title,
    string? Genre,
    decimal Rating
);