namespace MovieApi.Core.src.Domain.Entities;


public class Movie : IEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public decimal Rating { get; set; }
    public ICollection<Actor>? Actors { get; set; }
}
