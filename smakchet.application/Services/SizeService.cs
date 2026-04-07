using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Size;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ISize;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class SizeService(
        ISizeRepository repository,
        // IUnitOfWork unitOfWork,
        IMapper<Size, SizeReadDto, SizeDto, SizeUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : ISizeService
    {
        public Task<SizeReadDto> CreateAsync(SizeDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SizeReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(id, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Size", id),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(response);
            return result;
        }

        public async Task<ResponsePagingDto<SizeReadDto>> GetPagedAsync(PaginationQueryParams param)
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

        public Task<SizeReadDto?> UpdateAsync(int id, SizeUpdateDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
