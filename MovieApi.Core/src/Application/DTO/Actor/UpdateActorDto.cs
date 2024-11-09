namespace MovieApi.Core.src.Application.DTO.Actor;

public record UpdateActorDto(
    string Name,
    string Biography,
    DateTime Birthdate
);
