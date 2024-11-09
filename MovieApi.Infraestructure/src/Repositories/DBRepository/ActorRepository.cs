using Dapper;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;

namespace MovieApi.Infraestructure.src.Repositories.DBRepository;

public class ActorRepository : BaseRepository<Actor>, IActorRepository
{
    protected override string TableName => "actors";

    public ActorRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory)
    {
    }

    async Task<IEnumerable<Actor>> IActorRepository.GetActorByAgeRangeAsync(int minAge, int maxAge)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = @"
            SELECT *
            FROM actors
            WHERE EXTRACT(YEAR FROM AGE(CURRENT_DATE, birthdate))
            BETWEEN @MinAge AND @MaxAge";

        return await connection.QueryAsync<Actor>(query, new { MinAge = minAge, MaxAge = maxAge });
    }

    async Task<IEnumerable<Actor>> IActorRepository.GetActorsByMovieAsync(Guid movieId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = @"
            SELECT a.*
            FROM actors a
            INNER JOIN movie_actors ma ON a.id = ma.actor_id
            WHERE ma.movie_id = @MovieId";

        return await connection.QueryAsync<Actor>(query, new { MovieId = movieId });
    }

    async Task<int> IActorRepository.GetMovieCountByActorAsync(Guid actorId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = @"
            SELECT COUNT(*)
            FROM movie_actors
            WHERE actor_id = @ActorId";
        
        return await connection.ExecuteScalarAsync<int>(query, new { ActorId = actorId });
    }
}

