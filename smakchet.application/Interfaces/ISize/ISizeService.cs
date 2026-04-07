using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Size;

namespace smakchet.application.Interfaces.ISize
{
    public interface ISizeService
    {
        Task<ResponsePagingDto<SizeReadDto>> GetPagedAsync(PaginationQueryParams param);
        public Task<SizeReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<SizeReadDto?> UpdateAsync(int id, SizeUpdateDto payload, CancellationToken cancellationToken);
        public Task<SizeReadDto> CreateAsync(SizeDto payload, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
