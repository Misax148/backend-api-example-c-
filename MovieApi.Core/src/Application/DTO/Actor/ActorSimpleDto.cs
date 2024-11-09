namespace MovieApi.Core.src.Application.DTO.Actor;

public record ActorSimpleDto(
    Guid Id,
    string Name,
    DateTime Birthdate
);