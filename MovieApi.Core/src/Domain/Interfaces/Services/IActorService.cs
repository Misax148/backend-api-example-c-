using MovieApi.Core.src.Application.DTO.Actor;

namespace MovieApi.Core.src.Domain.Interfaces.Services;

public interface IActorService
{
    Task<ActorDto> CreateActorAsync(CreateActorDto actorDto);
    Task<ActorDto> UpdateActorAsync(Guid id, UpdateActorDto updateActorDto);
    Task<bool> DeleteActorAsync(Guid id);
    Task<ActorDto> GetActorByIdAsync(Guid id);
    Task<IEnumerable<ActorDto>> GetAllActorsAsync();
    Task<IEnumerable<ActorDto>> GetActorByMovieAsync(Guid movieId);
    Task<IEnumerable<ActorDto>> GetActorByAgeRangeAsync(int minAge, int maxAge);
    Task<int> GetMovieCountByActorAsync(Guid actorId);
}
