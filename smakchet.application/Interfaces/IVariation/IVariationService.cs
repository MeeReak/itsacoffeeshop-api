using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Variation;

namespace smakchet.application.Interfaces.IVariation
{
    public interface IVariationService
    {
        Task<ResponsePagingDto<VariationReadDto>> GetPagedAsync(PaginationQueryParams param);
        public Task<VariationReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<VariationReadDto?> UpdateAsync(int id, VariationUpdateDto payload, CancellationToken cancellationToken);
        public Task<VariationReadDto> CreateAsync(VariationDto payload, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
