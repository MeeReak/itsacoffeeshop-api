using smakchet.application.Interfaces;
using smakchet.dal.Models;

namespace smakchet.application.Repositories
{
    public class UnitOfWork(SmakchetContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => context.SaveChangesAsync(cancellationToken);
    }
}
