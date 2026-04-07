using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Variation;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IVariation;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class VariationService(
        IVariationRepository repository,
        // IUnitOfWork unitOfWork,
        IMapper<Variation, VariationReadDto, VariationDto, VariationUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : IVariationService
    {
        public Task<VariationReadDto> CreateAsync(VariationDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<VariationReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(id, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Variation", id),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(response);
            return result;
        }

        public async Task<ResponsePagingDto<VariationReadDto>> GetPagedAsync(PaginationQueryParams param)
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

        public Task<VariationReadDto?> UpdateAsync(int id, VariationUpdateDto payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
