using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ICoffeeLevel;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class CoffeeLevelService(
        ICoffeeLevelRepository repository,
        // IUnitOfWork unitOfWork,
        IMapper<CoffeeLevel, CoffeeLevelReadDto, CoffeeLevelDto, CoffeeLevelUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : ICoffeeLevelService
    {
        public Task<CoffeeLevelReadDto> CreateAsync(CoffeeLevelDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CoffeeLevelReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(id, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "CoffeeLevel", id),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(response);
            return result;
        }

        public async Task<ResponsePagingDto<CoffeeLevelReadDto>> GetPagedAsync(PaginationQueryParams param)
        {
            return await repository
                .Query()
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToPagedResultAsync(
                    param.Skip,
                    param.Top,
                    mapper.ToReadDto,
                    contextAccessor.HttpContext
                );
        }

        public Task<CoffeeLevelReadDto?> UpdateAsync(int id, CoffeeLevelUpdateDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
