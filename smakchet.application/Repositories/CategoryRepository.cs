using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.ICategory;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class CategoryRepository(SmakchetContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }


    public IQueryable<Category> Query()
    {
        return context.Categories;
    }
}