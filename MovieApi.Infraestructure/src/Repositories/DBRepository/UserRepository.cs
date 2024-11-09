using Dapper;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;

namespace MovieApi.Infraestructure.src.Repositories.DBRepository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    protected override string TableName => "users";

    public UserRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = "SELECT * FROM users WHERE email = @Email";
        return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = "SELECT COUNT(*) FROM users WHERE email = @Email";
        var count = await connection.ExecuteScalarAsync<int>(query, new { Email = email });
        return count == 0;
    }
}
