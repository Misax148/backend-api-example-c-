using MovieApi.Core.src.Domain.Entities;

namespace MovieApi.Core.src.Domain.Interfaces.Repositories;

public interface IActorRepository : IRepository<Actor>
{
    Task<IEnumerable<Actor>> GetActorsByMovieAsync(Guid movieId);
    Task<IEnumerable<Actor>> GetActorByAgeRangeAsync(int minAge, int maxAge);
    Task<int> GetMovieCountByActorAsync(Guid actorId);
}
