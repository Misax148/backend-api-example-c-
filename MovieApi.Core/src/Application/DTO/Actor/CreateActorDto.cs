namespace MovieApi.Core.src.Application.DTO.Actor;

public record CreateActorDto(
    string Name, 
    string Biography,
    DateTime Birthdate
);
