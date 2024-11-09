using MovieApi.Core.src.Application.DTO.Movie;

namespace MovieApi.Core.src.Domain.Interfaces.Services;

public interface IMovieService
{
    Task<MovieDto> CreateMovieAsync(CreateMovieDto movieDto);
    Task<MovieDto> UpdateMovieAsync(Guid id, UpdateMovieDto updateMovieDto);
    Task<bool> DeteleMovieAsync(Guid id);
    Task<MovieDto> GetMovieByIdAsync(Guid id);
    Task<IEnumerable<MovieDto>> GetAllMovies();
    Task<MovieDto> GetMovieWithActorsAsync(Guid movieId);
    Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genre);
    Task<IEnumerable<MovieDto>> GetMoviesByRatingAsync(decimal rating);
}
