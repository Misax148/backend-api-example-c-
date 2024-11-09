using Dapper;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;

namespace MovieApi.Infraestructure.src.Repositories.DBRepository;

public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    protected override string TableName => "movies";

    public MovieRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = "SELECT * FROM WHERE genre = @Genre";
        return await connection.QueryAsync<Movie>(query, new { Genre = genre });
    }

    public async Task<IEnumerable<Movie>> GetMoviesByRatingAsync(decimal rating)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = "SELECT * FROM movies WHERE rating >= @Rating";
        return await connection.QueryAsync<Movie>(query, new { Rating = rating });
    }

    public async Task<Movie?> GetMovieWithActorAsync(Guid movieId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = @"
            SELECT m.*, a.*
            FROM movies m
            LEFT JOIN movie_actors ma ON m.id = ma.movie_id
            LEFT JOIN actors a ON ma.actor_id = a.id
            WHERE m.id = @MovieId";

        var movieDictionary = new Dictionary<Guid, Movie>();
        var movies = await connection.QueryAsync<Movie, Actor, Movie>(
            query,
            (movie, actor) =>
            {
                if (!movieDictionary.TryGetValue(movie.Id, out var movieEntry))
                {
                    movieEntry = movie;
                    movieEntry.Actors = new List<Actor>();
                    movieDictionary.Add(movie.Id, movieEntry);
                }

                if (actor != null)
                {
                    movieEntry.Actors?.Add(actor);
                }

                return movieEntry;
            },
            new { MovieId = movieId },
            splitOn: "id"
        );

        return movies.FirstOrDefault();
    }
}
