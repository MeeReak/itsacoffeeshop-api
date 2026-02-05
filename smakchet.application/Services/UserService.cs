using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces.IUser;

namespace smakchet.application.Services
{
    public class UserService(IUserRepository repository, IUserMapper mapper, IHttpContextAccessor contextAccessor) : IUserService
    {
        public async Task CreateUserAsync(UserDto user, CancellationToken cancellationToken)
        {
            var mapped = mapper.ToEntity(user);
            await repository.AddAsync(mapped, cancellationToken);
        }



        public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);
            await repository.DeleteAsync(user, cancellationToken);
        }



        public async Task<UserReadDto?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToSource(user);
            return result;
        }



        public async Task<ResponsePagingDto<UserReadDto>> GetUsersPagedAsync(
            PaginationQueryParams param)
        {
            return await repository
                .Query()
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToPagedResultAsync(
                    param.Skip,
                    param.Top,
                    mapper.ToSource,
                    contextAccessor.HttpContext
                );
        }



        public async Task UpdateUserAsync(int userId, UserUpdateDto user, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(userId, cancellationToken);
            if (response == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);

            mapper.UpdateEntity(response, user);
            await repository.UpdateAsync(response, cancellationToken);
        }
    }
}
