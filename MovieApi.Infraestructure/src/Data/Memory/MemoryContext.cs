using MovieApi.Core.src.Domain.Entities;

namespace MovieApi.Infraestructure.src.Data.Memory;

public class MemoryContext
{
    public List<Movie> Movies { get; private set; }
    public List<Actor> Actors { get; private set; }
    public List<User> Users { get; private set; }
    public List<MovieActor> MovieActors { get; private set; }

    public MemoryContext()
    {
        Movies = new List<Movie>();
        Actors = new List<Actor>();
        Users = new List<User>();
        MovieActors = new List<MovieActor>();

        InitializeData();
    }

    private void InitializeData()
    {
        var inception = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Inception",
            ReleaseDate = new DateTime(2010, 7 , 16),
            Description = "Secrets i don't what movie is... ",
            Genre = "Sci-Fi",
            Rating = 3.4m
        };

        var batman = new Movie 
        {
            Id = Guid.NewGuid(),
            Title = "Batman",
            ReleaseDate = new DateTime(2004, 4, 1),
            Description = "Dark night on Chicago",
            Genre = "Action",
            Rating = 9.9m
        };

        Movies.Add(inception);
        Movies.Add(batman);

        var emmaStone = new Actor
        {
            Id = Guid.NewGuid(),
            Name = "Emma Stone",
            Biography = "United states actor movie girl",
            Birthdate = new DateTime(1998, 3, 3)
        };
    
        var jennifferLawrence = new Actor
        {
            Id = Guid.NewGuid(),
            Name = "Jennifer Lawrence",
            Biography = "United states actor movie girl from Texas",
            Birthdate = new DateTime(1995, 3, 3)
        };

        Actors.Add(emmaStone);
        Actors.Add(jennifferLawrence);

        var user1 = new User
        {
            Username = "User 1",
            Email = "email@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password 1"),
            Role = "user"
        };

        var user2 = new User
        {
            Username = "User 2",
            Email = "email2@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password 1"),
            Role = "user"
        };

        Users.Add(user1);
        Users.Add(user2);

        MovieActors.Add(new MovieActor 
        { 
            MovieId = inception.Id, 
            ActorId = emmaStone.Id 
        });
        
        MovieActors.Add(new MovieActor 
        { 
            MovieId = batman.Id, 
            ActorId = jennifferLawrence.Id 
        });
    }
}
