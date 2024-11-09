using MovieApi.Core.src.Domain.Entities;

namespace MovieApi.Core.src.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsEmailUniqueAsync(string email);
}
