using smakchet.application.DTOs;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Success;

namespace smakchet.application.Interfaces.ICategory
{
    public interface ICategoryService
    {
        Task<ResponsePagingDto<CategoryReadDto>> GetCategoryPagedAsync(PaginationQueryParams param);
        public Task<CategoryReadDto?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken);
        public Task UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken);
        public Task CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken);
        public Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
