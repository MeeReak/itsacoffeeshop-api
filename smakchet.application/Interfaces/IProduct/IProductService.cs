using smakchet.application.DTOs;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.Success;

namespace smakchet.application.Interfaces.IProduct
{
    public interface IProductService
    {
        Task<ResponsePagingDto<ProductReadDto>> GetProductPagedAsync(PaginationQueryParams param);
        public Task<ProductReadDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
        public Task<ProductReadDto?> UpdateProductAsync(int productId, ProductUpdateDto productDto, CancellationToken cancellationToken);
        public Task<ProductReadDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken);
        public Task DeleteProductAsync(int productId, CancellationToken cancellationToken);
    }
}
