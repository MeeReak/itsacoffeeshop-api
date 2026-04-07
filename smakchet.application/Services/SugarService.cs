using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Sugar;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ISugar;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class SugarService(
        ISugarRepository repository,
        // IUnitOfWork unitOfWork,
        IMapper<Sugar, SugarReadDto, SugarDto, SugarUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : ISugarService
    {
        public Task<SugarReadDto> CreateAsync(SugarDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SugarReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(id, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Sugar", id),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(response);
            return result;
        }

        public async Task<ResponsePagingDto<SugarReadDto>> GetPagedAsync(PaginationQueryParams param)
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

        public Task<SugarReadDto?> UpdateAsync(int id, SugarUpdateDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
