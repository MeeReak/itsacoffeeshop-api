using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces;
using smakchet.dal.Models;

namespace smakchet.application.Repositories
{
    public class BaseRepository<T>(SmakchetContext context) : IBaseRepository<T> where T : class
    {
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Set<T>().AsNoTracking().ToListAsync<T>(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<T> Query()
        {
            return context.Set<T>();
        }
    }
}
