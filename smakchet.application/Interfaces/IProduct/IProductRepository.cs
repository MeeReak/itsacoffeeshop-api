using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IProduct
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> Query();
        Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
