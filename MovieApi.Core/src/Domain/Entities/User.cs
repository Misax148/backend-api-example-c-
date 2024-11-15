namespace MovieApi.Core.src.Domain.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Role { get; set; }
}
