using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Product;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class ProductMapper : IProductMapper
    {
        public Product ToEntity(ProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder,
                IsFeatured = dto.IsFeatured,
                Price = dto.Price,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.Now
            };
        }

        public ProductReadDto ToReadDto(Product entity)
        {
            return new ProductReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                DisplayOrder = entity.DisplayOrder,
                ImageUrl = $"http://localhost:9000{entity.ImageUrl}" ,
                IsFeatured = entity.IsFeatured,
                CreatedAt = entity.CreatedAt
            };
        }

        public void UpdateEntity(Product entity, ProductUpdateDto dto)
        {
            entity.Name = dto.Name;
            entity.Price = dto.Price; 
            entity.DisplayOrder = dto.DisplayOrder;  
            entity.IsFeatured = dto.IsFeatured;  
            entity.Description = dto.Description;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
