using MovieApi.Core.src.Application.DTO.Movie;

namespace MovieApi.Core.src.Application.DTO.Actor;

public record ActorDto(
    Guid Id,
    string? Name,
    string? Biography,
    DateTime Birthdate,
    ICollection<MovieSimpleDto> Movies
);
