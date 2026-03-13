using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IUser;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class UserService(
        IUserRepository repository,
        IUnitOfWork unitOfWork,
        IMapper<User, UserReadDto, UserDto, UserUpdateDto> mapper,
        IHttpContextAccessor contextAccessor) : IUserService
    {
        public async Task<UserReadDto> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            var mapped = mapper.ToEntity(userDto);
            await repository.AddAsync(mapped, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.ToReadDto(mapped);
        }



        public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);
            repository.Delete(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }



        public async Task<UserReadDto?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);
            var result = mapper.ToReadDto(user);
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
                    mapper.ToReadDto,
                    contextAccessor.HttpContext
                );
        }



        public async Task<UserReadDto?> UpdateUserAsync(int userId, UserUpdateDto userDto, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                    ErrorCodeConstants.NotFound);

            mapper.UpdateEntity(user, userDto);
            repository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.ToReadDto(user);
        }
    }
}
