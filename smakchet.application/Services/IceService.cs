using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Ice;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IIce;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class IceService(
        IIceRepository repository,
        // IUnitOfWork unitOfWork,
        IMapper<Ice, IceReadDto, IceDto, IceUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : IIceService
    {
        public Task<IceReadDto> CreateAsync(IceDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IceReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(id, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Ice", id),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(response);
            return result;
        }

        public async Task<ResponsePagingDto<IceReadDto>> GetPagedAsync(PaginationQueryParams param)
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

        public Task<IceReadDto?> UpdateAsync(int id, IceUpdateDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
