using System.Data;

namespace MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}
