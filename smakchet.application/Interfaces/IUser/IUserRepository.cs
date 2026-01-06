using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserRepository
    {
        Task<ResponsePagingDto<UserReadDto>> GetUsersPagedAsync(PaginationQueryParams param);
        public Task<UserReadDto?> GetUserByIdAsync(int userId);
        public Task<UserReadDto> UpdateUserAsync(int userId, UserDto user);
        public Task<UserReadDto> CreateUserAsync(UserDto user);
        public Task DeleteUserAsync(int userId);
    }
}
