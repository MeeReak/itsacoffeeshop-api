using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Ice;

namespace smakchet.application.Interfaces.IIce
{
    public interface IIceService
    {
        Task<ResponsePagingDto<IceReadDto>> GetPagedAsync(PaginationQueryParams param);
        public Task<IceReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IceReadDto?> UpdateAsync(int id, IceUpdateDto payload, CancellationToken cancellationToken);
        public Task<IceReadDto> CreateAsync(IceDto payload, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
