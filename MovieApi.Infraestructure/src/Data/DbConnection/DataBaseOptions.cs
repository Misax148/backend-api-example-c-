namespace MovieApi.Infraestructure.src.Data.DbConnection;

public class DataBaseOptions
{
    public const string ConnectionStrings = nameof(ConnectionStrings);
    public string? DefaultConnection { get; set; }
}
