using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.Memory;

namespace MovieApi.Infraestructure.src.Repositories.BaseMemoryRepository;

public class UserMemoryRepository : BaseMemoryRepository<User>, IUserRepository
{
    public UserMemoryRepository(MemoryContext context) 
        : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        foreach (var user in GetDbSet())
        {
            if (user.Email!.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                return user;
            }
        }

        return await Task.FromResult<User?>(null);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        foreach (var user in GetDbSet())
        {
            if (user.Email!.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                return await Task.FromResult(false);
            }
        }

        return await Task.FromResult(true);
    }

    protected override List<User> GetDbSet()
    {
        return _context.Users;
    }
}
