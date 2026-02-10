using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ICategory
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
