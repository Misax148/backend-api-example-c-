using MovieApi.Core.src.Application.DTO.Actor;

namespace MovieApi.Core.src.Application.DTO.Movie;

public record MovieWithActorsDto(
    Guid Id,
    string Title,
    string Description,
    string Genre,
    decimal Rating,
    ICollection<ActorSimpleDto> Actors
);
