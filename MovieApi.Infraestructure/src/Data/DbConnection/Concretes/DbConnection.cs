using System.Data;
using Microsoft.Extensions.Options;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;
using Npgsql;


namespace MovieApi.Infraestructure.src.Data.DbConnection.Concretes;

public class DbConnection : IDbConnectionFactory
{
    private readonly DataBaseOptions _options;

    public DbConnection(IOptions<DataBaseOptions> options) 
    {
        _options = options.Value;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_options.DefaultConnection);
        await connection.OpenAsync();
        return connection;
    }
}
