using MovieApi.Core.src.Application.DTO.Actor;

namespace MovieApi.Core.src.Application.DTO.Movie;

public record MovieDto(
    Guid Id,
    string? Title,
    DateTime ReleaseDate,
    string? Description,
    string? Genre,
    decimal Rating,
    ICollection<ActorDto> Actors
);
