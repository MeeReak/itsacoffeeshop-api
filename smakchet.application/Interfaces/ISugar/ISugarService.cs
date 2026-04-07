using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.Sugar;

namespace smakchet.application.Interfaces.ISugar
{
    public interface ISugarService
    {
        Task<ResponsePagingDto<SugarReadDto>> GetPagedAsync(PaginationQueryParams param);
        public Task<SugarReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<SugarReadDto?> UpdateAsync(int id, SugarUpdateDto payload, CancellationToken cancellationToken);
        public Task<SugarReadDto> CreateAsync(SugarDto payload, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
