namespace MovieApi.Core.src.Application.DTO.Movie;

public record UpdateMovieDto(
    string Title,
    DateTime ReleaseDate,
    string Description,
    string Genre,
    decimal Rating
);
