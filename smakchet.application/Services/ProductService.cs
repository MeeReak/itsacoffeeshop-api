using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.IFileStorageService;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.Success;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Services;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    IMapper<Product, ProductReadDto, ProductDto, ProductUpdateDto> mapper,
    ILogger<ProductService> logger,
    IHttpContextAccessor contextAccessor) : IProductService
{
    public async Task<ProductReadDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken)
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

            await using var stream = productDto.File.OpenReadStream();
            mapped.ImageUrl =
                await fileStorageService.UploadFileAsync(stream, productDto.File.FileName, "products",
                    cancellationToken);

            await repository.AddAsync(mapped, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation(SuccessMessageConstants.Created, "Product");

            return mapper.ToReadDto(mapped);
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
            repository.Delete(existing);
            await unitOfWork.SaveChangesAsync(cancellationToken);
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
        IQueryable<Product> query = repository.Query();

        if (param.IsFeature == true)
            query = query.Where(c => c.IsFeatured);

        if (!string.IsNullOrWhiteSpace(param.Search))
            query = query.Where(c => c.Name.Contains(param.Search));

        query = query.OrderByDescending(c => c.IsFeatured)
            .ThenBy(c => c.DisplayOrder);

        var products = await query.ToPagedResultAsync(
            param.Skip,
            param.Top,
            mapper.ToReadDto,
            contextAccessor.HttpContext);

        logger.LogInformation(SuccessMessageConstants.Retrieved, "Product");
        return products;
    }


    public async Task<ProductReadDto?> UpdateProductAsync(
        int productId,
        ProductUpdateDto productDto,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(productId, cancellationToken);
        if (existing is null)
        {
            logger.LogError($"Product not found by Id {productId}");
            throw new NotFoundException($"Product not found by Id {productId}", ErrorCodeConstants.NotFound);
        }

        try
        {
            var newImageUrl = existing.ImageUrl;

            if (productDto.File is not null)
            {
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                    await fileStorageService.DeleteFileAsync(existing.ImageUrl, "products", cancellationToken);

                await using var stream = productDto.File.OpenReadStream();
                newImageUrl = await fileStorageService.UploadFileAsync(
                    stream,
                    productDto.File.FileName,
                    "products",
                    cancellationToken);
            }

            mapper.UpdateEntity(existing, productDto);
            existing.ImageUrl = newImageUrl;
            repository.Update(existing);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product updated successfully");

            return mapper.ToReadDto(existing);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateProduct operation failed");
            throw;
        }
    }
}