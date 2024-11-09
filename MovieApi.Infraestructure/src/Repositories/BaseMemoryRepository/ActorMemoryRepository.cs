using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.Memory;

namespace MovieApi.Infraestructure.src.Repositories.BaseMemoryRepository;

public class ActorMemoryRepository : BaseMemoryRepository<Actor>, IActorRepository
{
    public ActorMemoryRepository(MemoryContext context) 
        : base(context)
    {
    }

    public async Task<IEnumerable<Actor>> GetActorByAgeRangeAsync(int minAge, int maxAge)
    {
        var results = new List<Actor>();
        var currentDate = DateTime.UtcNow;

        foreach (var actor in GetDbSet())
        {
            var age = currentDate.Year - actor.Birthdate.Year;
            if (currentDate.DayOfYear < actor.Birthdate.DayOfYear)
            {
                age--;
            }

            if (age >= minAge && age <= maxAge)
            {
                results.Add(actor);
            }
        }

        return await Task.FromResult(results);
    }

    public async Task<IEnumerable<Actor>> GetActorsByMovieAsync(Guid movieId)
    {
        var results = new List<Actor>();
        var actorsInMovie = _context.MovieActors.Where(ma => ma.MovieId == movieId);

        foreach (var actor in GetDbSet())
        {
            if (actorsInMovie.Any(ma => ma.ActorId == actor.Id))
            {
                results.Add(actor);
            }
        }

        return await Task.FromResult(results);
    }

    public async Task<int> GetMovieCountByActorAsync(Guid actorId)
    {
        var count = _context.MovieActors.Count(ma => ma.ActorId == actorId);
        return await Task.FromResult(count);
    }

    protected override List<Actor> GetDbSet()
    {
        return _context.Actors;
    }
}
