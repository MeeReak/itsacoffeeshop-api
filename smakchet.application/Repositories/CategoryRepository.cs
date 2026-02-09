using smakchet.application.Interfaces.ICategory;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class CategoryRepository(SmakchetContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public IQueryable<Category> Query()
    {
        return context.Categories;
    }
}