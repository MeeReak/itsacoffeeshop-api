using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.CoffeeLevel;

namespace smakchet.application.Interfaces.ICoffeeLevel
{
    public interface ICoffeeLevelService
    {
        Task<ResponsePagingDto<CoffeeLevelReadDto>> GetPagedAsync(PaginationQueryParams param);
        public Task<CoffeeLevelReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<CoffeeLevelReadDto?> UpdateAsync(int id, CoffeeLevelUpdateDto payload, CancellationToken cancellationToken);
        public Task<CoffeeLevelReadDto> CreateAsync(CoffeeLevelDto payload, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
