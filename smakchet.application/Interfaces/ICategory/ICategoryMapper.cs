using smakchet.application.DTOs.Category;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ICategory
{
    public interface ICategoryMapper : IMapper<Category, CategoryReadDto, CategoryDto, CategoryUpdateDto>
    {
    }
}
