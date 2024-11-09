using MovieApi.Core.src.Domain.Entities;

namespace MovieApi.Core.src.Domain.Interfaces.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task<Movie?> GetMovieWithActorAsync(Guid movieId);
    Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
    Task<IEnumerable<Movie>> GetMoviesByRatingAsync(decimal rating);
}
