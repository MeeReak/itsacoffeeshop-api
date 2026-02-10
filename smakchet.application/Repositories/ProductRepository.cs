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


    public IQueryable<Product> Query()
    {
        return context.Products;
    }
}