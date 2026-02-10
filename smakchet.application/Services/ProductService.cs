using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.Success;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class ProductService(
        IProductRepository repository,
        IMapper<Product, ProductReadDto, ProductDto, ProductUpdateDto> mapper,
        ILogger<ProductService> logger,
        IHttpContextAccessor contextAccessor) : IProductService
    {
        public async Task CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByNameAsync(productDto.Name, cancellationToken);
            if (existing is not null)
            {
                logger.LogError(ErrorMessageConstants.AlreadyExists, "Product", productDto.Name);
                throw new DuplicateException(string.Format(ErrorMessageConstants.AlreadyExists, "Product", productDto.Name),
                    ErrorCodeConstants.Conflict);
            }

            try
            {
                var mapped = mapper.ToEntity(productDto);
                await repository.AddAsync(mapped, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Created, "Product");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "CreateProduct");
                throw;
            }
        }



        public async Task DeleteProductAsync(int productId, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(productId, cancellationToken);
            if (existing is null)
            {
                logger.LogError(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId));
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId),
                    ErrorCodeConstants.NotFound);
            }

            try
            {
                await repository.DeleteAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Deleted, "Product");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "DeleteProduct");
                throw;
            }
        }



        public async Task<ProductReadDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(productId, cancellationToken);
            if (existing is null)
            {
                logger.LogError(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId));
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId),
                    ErrorCodeConstants.NotFound);
            }

            var result = mapper.ToReadDto(existing);
            logger.LogInformation(SuccessMessageConstants.Retrieved, "Product");
            return result;
        }



        public async Task<ResponsePagingDto<ProductReadDto>> GetProductPagedAsync(PaginationQueryParams param)
        {
            var query = repository.Query()
                .OrderByDescending(c => c.IsFeatured)
                .ThenBy(c => c.DisplayOrder);


            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                query = (IOrderedQueryable<Product>)query.Where(c => c.Name.Contains(param.Search));
            }

            var products = await query
                .ToPagedResultAsync(
                    param.Skip,
                    param.Top,
                    mapper.ToReadDto,
                    contextAccessor.HttpContext);

            logger.LogInformation(SuccessMessageConstants.Retrieved, "Product");
            return products;
        }



        public async Task UpdateProductAsync(int productId, ProductUpdateDto productDto, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(productId, cancellationToken);
            if (existing is null)
            {
                logger.LogError(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId));
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", productId),
                    ErrorCodeConstants.NotFound);
            }

            try
            {
                mapper.UpdateEntity(existing, productDto);
                await repository.UpdateAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Updated, "Product");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateProduct");
                throw;
            }
        }
    }
}
