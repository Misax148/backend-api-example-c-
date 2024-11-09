using Dapper;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;

namespace MovieApi.Infraestructure.src.Repositories.DBRepository;

public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly IDbConnectionFactory _connectionFactory;
    protected abstract string TableName { get; }

    protected BaseRepository(IDbConnectionFactory connectionFactory) 
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(T item)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var properties = typeof(T).GetProperties()
            .Select(p => p.Name)
            .SkipLast(1);

        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));

        var query = $"INSERT INTO {TableName} ({columns}) VALUES ({values})";

        var result = await connection.ExecuteAsync(query, item);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = $"DELETE FROM {TableName} WHERE Id = @Id";

        var result = await connection.ExecuteAsync(query, new { Id = id });
        return result > 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = $"SELECT * FROM {TableName}";

        return await connection.QueryAsync<T>(query);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var query = $"SELECT * FROM {TableName} WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
    }

    public async Task<bool> UpdateAsync(T item)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id")
            .Select(p => $"{p.Name} = @{p.Name}")
            .SkipLast(1);

        var setClause = string.Join(", ", properties);

        var query = $"UPDATE {TableName} SET {setClause} WHERE Id = @Id";

        var result = await connection.ExecuteAsync(query, item);
        return result > 0;
    }
}
