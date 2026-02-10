using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class BaseRepository<T>(SmakchetContext context) : IBaseRepository<T>
    where T : class
{
    public Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Add(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync(
            new object[] { id },
            cancellationToken
        );
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}