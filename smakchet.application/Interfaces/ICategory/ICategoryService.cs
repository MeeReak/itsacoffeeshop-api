using smakchet.application.DTOs;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.DTOs.Success;

namespace smakchet.application.Interfaces.ICategory
{
    public interface ICategoryService
    {
        Task<ResponsePagingDto<CategoryReadDto>> GetCategoryPagedAsync(PaginationQueryParams param);
        Task<CategoryReadDto?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
        Task<CategoryReadDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken);
        Task<CategoryReadDto> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
