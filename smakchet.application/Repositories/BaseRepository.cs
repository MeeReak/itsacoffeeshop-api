using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces;
using smakchet.dal.Models;
using System.Linq.Expressions;

namespace smakchet.application.Repositories;

public class BaseRepository<T>(SmakchetContext context) : IBaseRepository<T>
    where T : class
{
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync(
            new object[] { id },
            cancellationToken
        );
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await context.Set<T>()
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>();
    }
}