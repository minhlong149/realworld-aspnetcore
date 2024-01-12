using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public abstract class Repository<T>(ConduitContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Add(entity);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return context.SaveChangesAsync(cancellationToken);
    }
}
