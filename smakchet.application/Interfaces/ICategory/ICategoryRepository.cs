using smakchet.application.DTOs.Category;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ICategory
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        IQueryable<Category> Query();
        Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
