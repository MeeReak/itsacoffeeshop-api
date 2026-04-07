using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class ProductRepository(SmakchetContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Products
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<List<Product>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
    {
        return await context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
}