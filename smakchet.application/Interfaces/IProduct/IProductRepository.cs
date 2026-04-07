using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IProduct
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Product>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken);
    }
}
