using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Success;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ICategory;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class CategoryService(
        ICategoryRepository repository,
        IMapper<Category, CategoryReadDto, CategoryDto, CategoryUpdateDto> mapper,
        ILogger<CategoryService> logger,
        IHttpContextAccessor contextAccessor) : ICategoryService
    {
        public async Task CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var mapped = mapper.ToEntity(categoryDto);
            try
            {
                await repository.AddAsync(mapped, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Created, "CreateCategory");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "CreateCategory");
                throw;
            }
        }



        public async Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(categoryId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Category", categoryId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "category", categoryId),
                    ErrorCodeConstants.NotFound);
            }
            try
            {
                await repository.DeleteAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Deleted, "DeleteCategory");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "DeleteCategory");
                throw;  
            }
        }



        public async Task<CategoryReadDto?> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(categoryId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Category", categoryId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "category", categoryId),
                    ErrorCodeConstants.NotFound);
            }
            var result = mapper.ToReadDto(existing);
            logger.LogInformation(SuccessMessageConstants.Retrieved, "Category");
            return result;
        }



        public async Task<ResponsePagingDto<CategoryReadDto>> GetCategoryPagedAsync(PaginationQueryParams param)
        {
            var categories = await repository.Query().AsNoTracking().OrderBy(u => u.DisplayOrder)
                .ToPagedResultAsync(param.Skip, param.Top, mapper.ToReadDto, contextAccessor.HttpContext);
            logger.LogInformation(SuccessMessageConstants.Retrieved, "Category");
            return categories;
        }



        public async Task UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(categoryId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Category", categoryId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "category", categoryId),
                    ErrorCodeConstants.NotFound);
            }

            try
            {
                mapper.UpdateEntity(existing, categoryDto);
                await repository.UpdateAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Updated, "UpdateCategory");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateCategory");
                throw;
            }
        }
    }
}
