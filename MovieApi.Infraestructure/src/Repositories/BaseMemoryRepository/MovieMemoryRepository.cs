using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.Memory;

namespace MovieApi.Infraestructure.src.Repositories.BaseMemoryRepository;

public class MovieMemoryRepository : BaseMemoryRepository<Movie>, IMovieRepository
{
    public MovieMemoryRepository(MemoryContext context) 
        : base(context)
    {
    }

    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
    {
        var results = new List<Movie>();
        foreach (var movie in _context.Movies)
        {
            if (movie.Genre!.Equals(genre, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(movie);
            }
        }
        return await Task.FromResult(results);
    }

    public async Task<IEnumerable<Movie>> GetMoviesByRatingAsync(decimal rating)
    {
        var results = new List<Movie>();
        foreach (var movie in _context.Movies)
        {
            if (movie.Rating >= rating)
            {
                results.Add(movie);
            }
        }
        return await Task.FromResult(results);
    }

    public async Task<Movie?> GetMovieWithActorAsync(Guid movieId)
    {
        var movie = await GetByIdAsync(movieId);
        if (movie == null)
        {
            return null;
        }

        var movieActor = new List<Actor>();
        foreach (var relation in _context.MovieActors)
        {
            if (relation.MovieId == movieId)
            {
                foreach (var actor in _context.Actors)
                {
                    if (actor.Id == relation.ActorId)
                    {
                        movieActor.Add(actor);
                        break;
                    }
                }
            }
        }

        movie.Actors = movieActor;
        return movie;
    }

    protected override List<Movie> GetDbSet()
    {
        return _context.Movies;
    }
}
