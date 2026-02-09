using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserService
    {
        Task<ResponsePagingDto<UserReadDto>> GetUsersPagedAsync(PaginationQueryParams param);
        public Task<UserReadDto?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
        public Task UpdateUserAsync(int userId, UserUpdateDto userDto, CancellationToken cancellationToken);
        public Task CreateUserAsync(UserDto userDto, CancellationToken cancellationToken);
        public Task DeleteUserAsync(int userId, CancellationToken cancellationToken);
    }
}
