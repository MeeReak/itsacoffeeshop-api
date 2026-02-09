using smakchet.application.DTOs.Category;
using smakchet.application.Interfaces.ICategory;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class CategoryMapper : ICategoryMapper
    {
        public Category ToEntity(CategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
            };
        }

        public CategoryReadDto ToReadDto(Category entity)
        {
            return new CategoryReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayOrder = entity.DisplayOrder,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }

        public void UpdateEntity(Category entity, CategoryUpdateDto dto)
        {
            entity.Name = dto.Name;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
