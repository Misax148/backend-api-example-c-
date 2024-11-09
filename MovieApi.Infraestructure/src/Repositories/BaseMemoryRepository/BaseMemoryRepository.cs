using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.Memory;

namespace MovieApi.Infraestructure.src.Repositories.BaseMemoryRepository;

public abstract class BaseMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly MemoryContext _context;
    protected abstract List<T> GetDbSet();

    protected BaseMemoryRepository(MemoryContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(T item)
    {
        if (item.Id == Guid.Empty)
        {
            item.Id = Guid.NewGuid();
        }

        var list = GetDbSet();
        list.Add(item);
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var list = GetDbSet();
        var itemToDelete = await GetByIdAsync(id);

        if (itemToDelete == null)
        {
             return false;
        }

        list.Remove(itemToDelete);
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult(GetDbSet());
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        var list = GetDbSet();
        foreach (var item in list)
        {
            if (item.Id == id)
            {
                return await Task.FromResult(item);
            }
        }
        return null;
    }

    public async Task<bool> UpdateAsync(T item)
    {
        var list = GetDbSet();
        var existingItem = await GetByIdAsync(item.Id);

        if (existingItem == null)
        {
            return false;
        }

        list.Remove(existingItem);
        list.Add(item);

        return await Task.FromResult(true);
    }
}
