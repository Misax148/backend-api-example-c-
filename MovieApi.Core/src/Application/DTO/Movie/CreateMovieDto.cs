namespace MovieApi.Core.src.Application.DTO.Movie;

public record CreateMovieDto(
    string Title,
    DateTime ReleaseDate,
    string Description,
    string Genre,
    decimal Rating
);
