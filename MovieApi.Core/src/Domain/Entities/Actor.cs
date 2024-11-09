namespace MovieApi.Core.src.Domain.Entities;

public class Actor : IEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Biography { get; set; }
    public DateTime Birthdate { get; set; }
    public ICollection<Movie>? Movies { get; set; } 
}
