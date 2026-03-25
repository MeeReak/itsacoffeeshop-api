using Microsoft.AspNetCore.Http;
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
        IUnitOfWork unitOfWork,
        IMapper<Category, CategoryReadDto, CategoryDto, CategoryUpdateDto> mapper,
        ILogger<CategoryService> logger,
        IHttpContextAccessor contextAccessor) : ICategoryService
    {
        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByNameAsync(categoryDto.Name, cancellationToken);
            if (existing is not null)
            {
                logger.LogError(ErrorMessageConstants.AlreadyExists, "Category", categoryDto.Name);
                throw new DuplicateException(string.Format(ErrorMessageConstants.AlreadyExists, "Category", categoryDto.Name),
                    ErrorCodeConstants.Conflict);
            }

            try
            {
                var mapped = mapper.ToEntity(categoryDto);
                await repository.AddAsync(mapped, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Created, "Category");
                return mapper.ToReadDto(mapped);
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
            if (existing.IsActive == true)
            {
                logger.LogError(ErrorMessageConstants.GeneralUnexpectedError);
                throw new BadRequestException(string.Format(ErrorMessageConstants.GeneralUnexpectedError),
                    ErrorCodeConstants.BadRequest);
            }

            try
            {
                repository.Update(existing);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Deleted, "Category");
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
            IQueryable<Category> query = repository.Query()
                .Where(c => c.IsActive);

            query = query.OrderBy(c => c.DisplayOrder);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                query = query.Where(c => c.Name.Contains(param.Search));
            }

            var categories = await query
                .ToPagedResultAsync(
                    param.Skip,
                    param.Top,
                    mapper.ToReadDto,
                    contextAccessor.HttpContext);

            logger.LogInformation(SuccessMessageConstants.Retrieved, "Category");
            return categories;
        }



        public async Task<CategoryReadDto?> UpdateCategoryAsync(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken)
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
                repository.Update(existing);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Updated, "Category");
                return mapper.ToReadDto(existing);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateCategory");
                throw;
            }
        }
    }
}
